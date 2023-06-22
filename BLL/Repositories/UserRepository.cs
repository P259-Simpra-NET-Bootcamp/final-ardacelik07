using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.IRepositories;
using DAL;
using DAL.DTOs.Dtos;
using DAL.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(ApiDbContext context, ILogger logger, UserManager<IdentityUser> userManager) : base(context, logger)
        {
            _userManager = userManager;

        }

        public  async Task<IEnumerable<User>> AllUser()
        {
            try
            {
                return await dbSet.Where(x => x.Status != 0).OrderBy(o => o.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method error", typeof(UserRepository));

                return new List<User>();
            }
        } 

        public override async Task<User> GetById(Guid id)
        {
            try
            {

                var user = await dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetById method error", typeof(UserRepository));

                return null;
            }
        }

        public string GetUserTypeName(Guid id)
        {

         

            var users = _userManager.Users.ToList();

            return "";
     
        }

     

        public async Task<User> GetByAspNetUserId(string id)
        {
            try
            {

                var user = await dbSet.Where(x => x.AspNetUserId == id).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetById method error", typeof(UserRepository));

                throw new Exception(ex.Message);

            }
        }

        public async Task<User> GetByEmailAddress(string email)
        {
            try
            {

                var user = await dbSet.Where(x => x.Email == email).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetByEmail method error", typeof(UserRepository));

                throw new Exception(ex.Message);

            }
        }

       
    }
}
