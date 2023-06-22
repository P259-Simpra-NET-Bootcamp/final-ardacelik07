using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> All();

        Task<T> GetById(Guid id);
        T GetByIdWithInteger(int id);

        Task<bool> Add(T entity);
        Task<bool> Delete(Guid id);

        Task<bool> DeleteByIdWithInteger(int id);

        Task<bool> Upsert(T entity);
    }
}
