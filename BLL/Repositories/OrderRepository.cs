using BLL.Configuration;
using BLL.IConfiguration;
using BLL.IRepositories;
using DAL;
using DAL.DTOs.Dtos;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BLL.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        protected readonly ApiDbContext dbContext;
        

       

        
        
       


        public OrderRepository(ApiDbContext context, ILogger logger) : base(context, logger)
        {
            dbContext = context;
            

          



        }

      

        public Order CreateOrder(Guid id)
        {

            try
            {



                var userıd = dbContext.Set<User>().Include(x => x.Orders).Where(x => x.Id == id).FirstOrDefault();


                if (userıd.Orders.Count == 0)
                {
                    //Sisteme kayıtlı olan kullanıcının herhangi bir basketi (orderı) olup olmadıgını kontrol ediyor daha once daha sistemde hiç basket olusturmamıs ise yeni bir basket olusturuldu.

                    var order = new Order()
                    {
                        UserId = id,
                        TotalPrice = 0,
                        isValid = 1,
                        isCompletedSuccesfully = 0,
                        SiparişNo = GenerateOrderNo()



                    };


                    dbSet.Add(order);
                    dbContext.SaveChanges();

                    return order;


                }

                var validOrder = dbSet.Where(x => x.isValid == 1 && x.UserId == userıd.Id).Include(x => x.OrderDetails).FirstOrDefault();

                //burda ise kullanıcının basketlerinden aktif basket olup olmadıgı kontrol ediliyor. kullanıcıların sadece bir tane aktif basketi ve yahut orderı bulunabilir. herhangi aktif bir basketi yoksa
                // geçmişte yapılan orderların isvalid degerini 0 a cekiyoruz. aktif bir basketi yoksa basketi olmadıgı için yeni basket olusturuluyor.

                if (validOrder == null)
                {

                    var order = new Order()
                    {
                        UserId = id,
                        TotalPrice = 0,
                        isValid = 1,
                        isCompletedSuccesfully = 0,
                        SiparişNo = GenerateOrderNo()



                    };

                    dbSet.Add(order);
                    dbContext.SaveChanges();

                    return order;
                }
                else
                {

                    return null;
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} createordermethod method error", typeof(GenericRepository<OrderRepository>));
                throw new Exception(ex.Message);

            }




        }

       

        public Order GetTheCurrentOrder(Guid id)
        {
            //kullanıcının zaten sadece aktif olan bir basketi olabilcegi icin onun sorgusunu atıyoruz.
            var validOrder = dbSet.Where(x => x.isValid == 1 && x.UserId ==id).Include(x => x.OrderDetails).ThenInclude(x => x.Product).FirstOrDefault();

            return validOrder;
        }

        public List<Order> UserPreviousOrders(Guid id)
        {
            // geçmişte yaptıgı siparişler icin sadece isvalidi 0 olanları getirmemiz yeterli.
            var validOrder = dbSet.Where(x => x.isValid == 0 && x.isCompletedSuccesfully ==1 && x.UserId == id).Include(x => x.OrderDetails).ThenInclude(x => x.Product).ToList();


            return validOrder;
        }

        public Order AddItemToTheOrder(OrderDto OrderDto)
        {
            try
            {
                var userıd = dbContext.Set<User>().Include(x => x.Orders).Where(x => x.Id == OrderDto.Id).FirstOrDefault();

            var validOrder = dbSet.Where(x => x.isValid == 1 && x.UserId == userıd.Id).Include(x => x.OrderDetails).FirstOrDefault();

            //kullanıcın  aktif orderını bulup sepete sipariş eklemesi yapıyoruz. orderdetails = orderitems diyebiliriz. kullanıcılar ilk basketlerini oluşturup ondan sonra sepete ürün ekleyebilmektedir.

            if (validOrder != null)
            {
                var orderDetails = new OrderDetails()
                {
                    OrderId = validOrder.Id,
                    Quantity = OrderDto.Quantity,
                    ProductId = OrderDto.ProductId


                };

                validOrder.TotalPrice = (OrderDto.price * OrderDto.Quantity) + validOrder.TotalPrice;
                validOrder.OrderDetails.Add(orderDetails);
                dbContext.SaveChanges();
                return validOrder;
            }
            return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} additemtoorder method error", typeof(GenericRepository<OrderRepository>));
                throw new Exception(ex.Message);

            }


        }
        private string GenerateOrderNo()
        {
            Random random = new Random();
            int random_number = random.Next(100000, 1000000);

            return random_number.ToString();
        }


    }
}
