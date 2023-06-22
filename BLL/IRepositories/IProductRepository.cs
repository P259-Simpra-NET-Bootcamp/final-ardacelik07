using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepositories
{
    public interface   IProductRepository : IGenericRepository<Product>
    {
        Product stockControl(int id, string action,int amount);

        List<Product> InStock(bool valid);
    }
}
