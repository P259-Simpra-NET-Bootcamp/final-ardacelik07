using BLL.IRepositories;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class CategoryRepository  : GenericRepository<Category>, ICategoryRepository
    
    {


        protected readonly ApiDbContext dbContext;
        public CategoryRepository(ApiDbContext context, ILogger logger) : base(context, logger)
        {
            dbContext = context;

        }

        public Category CategoryGetByIdWithIncludes(int id)
        {
            var query = dbSet.Include(x =>x.Products);
           
            return query.FirstOrDefault(x => x.Id == id);
        }

        public List<Category> GetAllWithIncludes()
        {
            var query = dbSet.Include(x => x.Products);
            return query.ToList();  
        }
    }
}
