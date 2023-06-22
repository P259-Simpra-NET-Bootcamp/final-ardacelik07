using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.IRepositories;
using DAL;
using DAL.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLL.Repositories
{
    public class PasswordResetRepository : GenericRepository<PasswordReset>, IPasswordResetRepository
    {
      


        public PasswordResetRepository(ApiDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;

        }

        public string  GetByEmailAddressWith(Guid id)
        {
            try{
                var data = (from u in _context.User
                            join ut in _context.PasswordReset
                            on u.Id equals ut.UserID
                            where u.Id == id
                            select u.Email).FirstOrDefault();
                return data;

            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetByEmailadresswith method error", typeof(PasswordResetRepository));

                throw new Exception(ex.Message);
            }
           

        }
    }
}
