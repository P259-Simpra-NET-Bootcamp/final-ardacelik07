using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepositories
{
   public interface  IOrderDetailRepository : IGenericRepository<OrderDetails>
    {
        List<OrderDetails> GetOrderDetailsByOrderIdWithInclude(int id);
    }
}
