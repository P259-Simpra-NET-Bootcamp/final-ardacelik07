using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.IRepositories;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApiDbContext _context;
        protected DbSet<T> dbSet;
        

        protected readonly ILogger _logger;

        public GenericRepository(ApiDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            this.dbSet = _context.Set<T>();
          
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);

            return true;
        }

        public List<T> All()
        {
            return  dbSet.ToList();
        }

        public virtual async Task<bool> Delete(Guid id )
        {
            try
            {
                var entity = dbSet.Find(id);
                dbSet.Remove(entity);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error", typeof(GenericRepository<T>));
                throw new Exception(ex.Message);
            }
           
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Upsert(T entity)
        {
            try
            {
                dbSet.Update(entity);

                return true;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} upsert method error", typeof(GenericRepository<T>));
                throw new Exception(ex.Message);
            }
             

        }

        public virtual async  Task<bool> DeleteByIdWithInteger(int id)
        {
            try
            {
                var entity = dbSet.Find(id);
                dbSet.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error", typeof(GenericRepository<T>));
                throw new Exception(ex.Message);
            }
        }

        public T GetByIdWithInteger(int id)
        {
            return  dbSet.Find(id);
        }
    }
}
