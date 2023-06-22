using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTOs.Dtos;
using DAL.Models;


namespace BLL.IRepositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
       
        string GetUserTypeName(Guid id);
        Task<User> GetByAspNetUserId(string id);

        Task<User> GetByEmailAddress(string email);
        Task<IEnumerable<User>> AllUser();

        


    }
}
