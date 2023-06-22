using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;


namespace BLL.IRepositories
{
    public interface IPasswordResetRepository : IGenericRepository<PasswordReset>
    {
       public string GetByEmailAddressWith(Guid id);
    }
}
