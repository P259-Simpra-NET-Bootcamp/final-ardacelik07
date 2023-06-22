using BLL.IRepositories;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetails>, IOrderDetailRepository
    {
        protected readonly ApiDbContext dbContext;








        public OrderDetailRepository(ApiDbContext context, ILogger logger) : base(context, logger)
        {
            dbContext = context;





        }
        public List<OrderDetails> GetOrderDetailsByOrderIdWithInclude(int id)
        {
            var productItems = dbSet.Where(x => x.OrderId == id).Include(x => x.Product).ToList();
            return productItems;
        }
    }
}
