using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BLL.IConfiguration;
using BLL.Repositories;
using DAL;
using DAL.Configuration;
using DAL.DTOs.Dtos;
using DAL.DTOs.Requests;
using DAL.DTOs.Responses;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _uof;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApiDbContext _apiDbContext;
        private readonly IMapper _mapper;


        private string UserId = "";

        public UserController(ILogger<UserController> logger, IUnitOfWork uof, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
            ApiDbContext apiDbContext, IMapper mapper)
        {
            _logger = logger;
            _uof = uof;
            _userManager = userManager;
            _apiDbContext = apiDbContext;

            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [Route("CreateUsers")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest user)
        {
            var value = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var adminemail = User.FindFirstValue(ClaimTypes.Email);
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "This email already exists" }, Success = false });
                }

                var newUser = new IdentityUser() { Email = user.email, UserName = user.Name + user.LastName };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);


                if (isCreated.Succeeded)
                {

                    var appUser = new User()
                    {


                        Name = user.Name,
                        LastName = user.LastName,
                        AspNetUserId = newUser.Id,
                        Email = user.email,
                        CreatedBy = adminemail,
                        Role = "user"


                    };

                    await _uof.UserRepository.Add(appUser);
                    await _uof.CompleteAsync();



                    return Ok(appUser);
                }
                else
                {
                    return BadRequest(new RegistrationResponse() { Errors = isCreated.Errors.Select(x => x.Description).ToList(), Success = false });
                }
            }

            return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "Invalid User Credentials" }, Success = false });
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("UserDelete/{email}")]
        public async Task<IActionResult> UserDelete(string email)
        {
            try
            {

                var existingUser = await _userManager.FindByEmailAsync(email);
                var deleted = await _userManager.DeleteAsync(existingUser);

                if (deleted.Succeeded)
                {
                    var deleteuser = await _uof.UserRepository.GetByEmailAddress(email);
                    await _uof.UserRepository.Delete(deleteuser.Id);

                    await _uof.CompleteAsync();

                }
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }










        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [Route("UserUpdate")]
        public async Task<IActionResult> UserUpdate([FromBody] UserUpdateRequest user)
        {
            var adminemail = User.FindFirstValue(ClaimTypes.Email);

            var existingUser = await _userManager.FindByEmailAsync(user.Email);


            var UserUpdate = await _uof.UserRepository.GetByEmailAddress(existingUser.Email);

            var UserUpdateUm = await _userManager.FindByEmailAsync(UserUpdate.Email);

            if (UserUpdate.Role == "SuperAdmin")
            {
                return BadRequest("you are not able to update superAdmin");

            }

            var UserUpdateUmDto = new IdentityUser() { Email = user.Email, UserName = user.Name + user.LastName, PhoneNumber = user.PhoneNumber };


            UserUpdateUm.Email = UserUpdateUmDto.Email;
            UserUpdateUm.UserName = UserUpdateUmDto.UserName;
            UserUpdateUm.PhoneNumber = UserUpdateUmDto.PhoneNumber;


            var result = await _userManager.UpdateAsync(UserUpdateUm);



            if (result.Succeeded)
            {
                UserUpdate.Address = user.Address;
                UserUpdate.PhoneNumber = user.PhoneNumber;
                UserUpdate.Name = user.Name;
                UserUpdate.Email = user.Email;
                UserUpdate.LastName = user.LastName;

                UserUpdate.Role = user.Roles;
                UserUpdate.UpdatedBy = adminemail;
                UserUpdate.UpdatedAt = DateTime.UtcNow;


                await _uof.UserRepository.Upsert(UserUpdate);

                await _uof.CompleteAsync();

                return Ok(UserUpdate);

            }
            else
            {
                return BadRequest(new RegistrationResponse() { Errors = result.Errors.Select(x => x.Description).ToList(), Success = false });
            }












        }




        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {

            var users = await _uof.UserRepository.AllUser();


            if (users == null)
                return NotFound();

            return Ok(users);
        }




        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<ActionResult<forgetPasswordDto>> PasswordReset([FromBody] forgetPasswordDto User)
        {
            try
            {
                var existingUser = await _uof.UserRepository.GetByEmailAddress(User.Email);
                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse() { Errors = new List<string>() { "User does not exist" }, Success = false });
                }
                Random random = new Random();
                int random_number = random.Next(100000, 1000000);
                var PasswordResetUser = new PasswordReset()
                {

                    UserID = existingUser.Id,
                    EndDate = DateTime.Now.AddHours(48.00),
                    Code = random_number.ToString(),
                    IsUsed = false,

                };

                await _uof.PasswordResetRepository.Add(PasswordResetUser);
                await _uof.CompleteAsync();


                var id = PasswordResetUser.Id.ToString();


                var callback = User.DomainLink + "/passwordReset/" + id;




                await _uof.Mail.SendEmailForForgotPassword(existingUser.Email, existingUser.Name, PasswordResetUser.Code, callback); // try catch in inside of method




                return new forgetPasswordDto
                {
                    Email = User.Email,
                    ClientURI = callback,
                    Code = PasswordResetUser.Code,

                };

            }
            catch (Exception ex)
            {
                return BadRequest(new RegistrationResponse() { Errors = new List<string>() { ex.Message }, Success = false });
            }









        }


        [HttpGet]
        [Route("PasswordResetgetbyıd/{id}")]
        public async Task<IActionResult> PasswordResetGetById(Guid id)
        {
            var a = await _apiDbContext.PasswordReset.FirstOrDefaultAsync(p => p.Id == id);

            return Ok(a);

        }
        [HttpGet]
        [Route("GetUsersByEmail/{email}")]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {


            var user = await _uof.UserRepository.GetByEmailAddress(email);


            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("AddBalanceFromCreditCard")]
        public async Task<IActionResult> AddBalanceFromCreditCard(AddBalanceWithCreditCard AddBalanceWithCreditCard)
        {
            var email = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _uof.UserRepository.GetByEmailAddress(email);

            if (user == null)
            {

                return BadRequest("you should register or logged in");
            }
            var expireDate = Convert.ToDateTime(AddBalanceWithCreditCard.expireDate).Date;

            string isValidCardNumber = @"^4[0-9]{12}(?:[0-9]{3})?$";
            if (Regex.IsMatch(AddBalanceWithCreditCard.CardNumber, isValidCardNumber) && AddBalanceWithCreditCard.CVC < 1000 && expireDate > DateTime.Now.Date)
            {
                user.balance = user.balance + AddBalanceWithCreditCard.amount;
                var result = _uof.UserRepository.Upsert(user);
                await _uof.CompleteAsync();

                return Ok("balance added succesfully");
            }

            return BadRequest("invalid card informations");
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("MyPoints")]
        public async Task<IActionResult> MyPoints()
        {
            var email = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _uof.UserRepository.GetByEmailAddress(email);

            if (user == null)
            {
                return BadRequest(
              "you should login first"

              );

             


            }
            string userpoint = user.point.ToString();
             

            return Ok("you have " + userpoint + " points");
        }
    }
}
