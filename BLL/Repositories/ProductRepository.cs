using BLL.IRepositories;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public  class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public const string increase = "increase";
        public const string decrease = "decrease";
       

        public ProductRepository(ApiDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;

        }

        public List<Product> InStock(bool valid)
        {
            // boolean türde ürünlerin aktif olup olmadığı durumunu sorguluyoruz. true durumunda tüm aktif olarak bulunan urunler getirilir. stokta olmayan ürünlerde aktif degildir.
            var ProductInStock = dbSet.Where(x => x.IsValid == valid).ToList();

            return ProductInStock;
        }

        public Product stockControl(int id, string action, int amount)
        {
            // bu method da stok kontolu yapabiliriz. yapmak istedigimiz islemi query e girerek (decrease increase) ve miktarı girerek stok işlemlerini yapabilir. ürünleri artırıp azaltabilirz.
            try
            {
                var product = dbSet.Find(id);


                switch (action)
                {
                    case increase:
                        product.Stock = product.Stock + amount;
                        product.IsValid = true;

                        dbSet.Update(product);

                        break;
                    case decrease:

                        
                        product.Stock = product.Stock - amount;
                        if(product.Stock< 0)
                        {
                            return null;
                        }

                        if(product.Stock == 0 )
                        {
                            product.IsValid = false;
                        }
                        else
                        {
                            product.IsValid = true;
                        }
                       

                        dbSet.Update(product);
                        break;
                    default:

                        break;
                }


                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} stockcontrollererror", typeof(ProductRepository));
                throw new Exception(ex.Message);
            }

        }
    }
}
