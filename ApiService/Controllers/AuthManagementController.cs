using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.Configuration;
using BLL.IConfiguration;
using DAL;
using DAL.Configuration;
using DAL.DTOs.Requests;
using DAL.DTOs.Responses;
using DAL.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
       
        private readonly ApiDbContext _apiDbContext;

        private readonly JwtConfig _jwtConfig;

  
        private readonly IUnitOfWork _uof;

        public AuthManagementController(
            IUnitOfWork uof,
            UserManager<IdentityUser> userManager,
            ApiDbContext apiDbContext,
            IOptionsMonitor<JwtConfig> optionsMonitor
            )
        {
            _uof = uof;
            _userManager = userManager;
            _apiDbContext = apiDbContext;
            _jwtConfig = optionsMonitor.CurrentValue;
         
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null )
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "This email already exists" }, Success = false });
                }

                var newUser = new IdentityUser() { Email = user.Email, UserName = user.Name + user.LastName };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {

                    var appUser = new User()
                    {
                        Name = user.Name,
                        LastName = user.LastName,
                        AspNetUserId = newUser.Id,
                        Email = user.Email,
                        Role = "user"
                       
                    };
                    

                    await _apiDbContext.User.AddAsync(appUser);
                    
                    var jwtToken = await GenerateJwtToken(newUser,appUser.Role);

                    return Ok(jwtToken);
                }
                else
                {
                    return BadRequest(new RegistrationResponse() { Errors = isCreated.Errors.Select(x => x.Description).ToList(), Success = false });
                }
            }

            return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "Invalid User Credentials" }, Success = false });
        }
        [HttpPost]
        [Route("RegisterForBeingSuperAdmin")]
        public async Task<IActionResult> RegisterSuperAdmin([FromBody] UserRegistrationRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "This email already exists" }, Success = false });
                }

                var newUser = new IdentityUser() { Email = user.Email, UserName = user.Name + user.LastName };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {

                    var appUser = new User()
                    {
                        Name = user.Name,
                        LastName = user.LastName,
                        AspNetUserId = newUser.Id,
                        Email = user.Email,
                        Role = "SuperAdmin"

                    };


                    await _apiDbContext.User.AddAsync(appUser);

                    var jwtToken = await GenerateJwtToken(newUser, appUser.Role);

                    return Ok(jwtToken);
                }
                else
                {
                    return BadRequest(new RegistrationResponse() { Errors = isCreated.Errors.Select(x => x.Description).ToList(), Success = false });
                }
            }

            return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "Invalid User Credentials" }, Success = false });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("AdminRegisterBySuperAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegistrationRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "This email already exists" }, Success = false });
                }

                var newUser = new IdentityUser() { Email = user.Email, UserName = user.Name + user.LastName };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {

                    var appUser = new User()
                    {
                        Name = user.Name,
                        LastName = user.LastName,
                        AspNetUserId = newUser.Id,
                        Email = user.Email,
                        Role = "Admin"

                    };


                    await _apiDbContext.User.AddAsync(appUser);

                    var jwtToken = await GenerateJwtToken(newUser, appUser.Role);

                    return Ok(jwtToken);
                }
                else
                {
                    return BadRequest(new RegistrationResponse() { Errors = isCreated.Errors.Select(x => x.Description).ToList(), Success = false });
                }
            }

            return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "Invalid User Credentials" }, Success = false });
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                

                if (existingUser == null )
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "User does not exist" }, Success = false });
                }
                var userforexist = await _uof.UserRepository.GetByEmailAddress(existingUser.Email);

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "User password is wrong" }, Success = false });
                }

                var jwtToken = await GenerateJwtToken(existingUser,userforexist.Role);


                return Ok(jwtToken);
            }
            else
            {
                return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "Enter user email and password" }, Success = false });
            }


        }

     
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordRequest user)
        {
            
            if (ModelState.IsValid)
            {

                var finduserıd = await _uof.UserRepository.GetByEmailAddress(user.email);
                var PasswordResetUser = await _apiDbContext.PasswordReset.Where(x => x.UserID == finduserıd.Id).FirstOrDefaultAsync();
              var email =  _uof.PasswordResetRepository.GetByEmailAddressWith(PasswordResetUser.UserID);  // try catch in inside of method
                var existingUser = await _userManager.FindByEmailAsync(email); // try cath in inside of method

                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "User does not exist" }, Success = false });
                }
                
               if (  PasswordResetUser.IsUsed == true)
                {

                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "code is already used" }, Success = false });
                }

                if (PasswordResetUser.Code.Equals(user.Code))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

                    var isPasswordResetted = await _userManager.ResetPasswordAsync(existingUser, token, user.Password);
                    if (isPasswordResetted.Succeeded)
                    {
                        
                        PasswordResetUser.IsUsed = true;

                        await _uof.PasswordResetRepository.Upsert(PasswordResetUser); // try catch in inside
                        await _uof.CompleteAsync(); // try cath in inside of method
                    }



                    if (!isPasswordResetted.Succeeded)
                    {
                        return BadRequest(new RegistrationResponse() { Errors = isPasswordResetted.Errors.Select(x => x.Description).ToList(), Success = false });

                    }

                    return Ok();

                }
                else
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "Code is wrong" }, Success = false });

                }

            }
                    
            else
            {
                return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "Enter user email and password" }, Success = false });
            }


        }

        



        [HttpGet]
        [Route("GetUser/{id}")]
        public async Task<IActionResult> GetUser(String id)
        {

            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
            {
                return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "User does not exist" }, Success = false });
            }

            return Ok(existingUser);



        }

       

      

        private async Task<AuthResult> GenerateJwtToken(IdentityUser user,string userrole)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                     new Claim(ClaimTypes.Role,userrole),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                   
                   
                }
                
                ),

                Expires = DateTime.UtcNow.AddDays(14),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

           
            await _apiDbContext.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                UserId = user.Id
            };
        }

      

    }
}
