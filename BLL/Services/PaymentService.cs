using BLL.IConfiguration;
using BLL.Repositories;
using DAL;
using DAL.DTOs.Dtos;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uof;
        private readonly IHttpContextAccessor _httpContextAccessor;
       
        protected readonly ILogger<PaymentService> _logger;
        public PaymentService(IUnitOfWork uof, IHttpContextAccessor httpContextAccessor, ApiDbContext db, IGiftCardService giftCardService, ILogger<PaymentService> logger)
        {
            _uof= uof;
            _httpContextAccessor = httpContextAccessor;
            
            _logger = logger;

        }
        public async Task<string> ComplateOrder(PaymentParams param)
        {
            


            try
            {
                
                var order = _uof.OrderRepository.GetTheCurrentOrder(param.Id);

                if(order == null)
                {
                    return "you should create order first";
                }

                var user = await _uof.UserRepository.GetById(param.Id);

                int ifonlykupontenough = 0;
                int ifonlypointenough = 0;
                var earnpoint = order.TotalPrice;



                decimal totalpointforthisorder = 0;
                var productItems = _uof.OrderDetailRepository.GetOrderDetailsByOrderIdWithInclude(order.Id);

                if(productItems.Count == 0) {

                    return "you should add item to basket for payment";
                }
                // ilk olarak ürünün stokta olup olmadığı kontrol ediliyor.

                foreach (var item in productItems)
                {
                    if (item.Product.Stock <= 0)
                    {
                        item.Product.IsValid = false;
                        order.isValid = 0;
                        await _uof.ProductRepository.Upsert(item.Product);
                        await _uof.CompleteAsync();

                        return item.Product.Name + " is not in stock  you should create a new basket";
                    }
                    item.Product.Stock = item.Product.Stock - item.Quantity;
                    await _uof.ProductRepository.Upsert(item.Product);

                }
                
                order.UsedGiftCode = user.GiftCode;
               
                // kullanıcının kuponunun olup olmaması kontrol ediliyor.
                if (user.GiftCode.Equals(param.kupon) && user.GiftCodeExpireDate > DateTime.UtcNow && user.GiftCodeIsValid == 1)
                {
                    if (user.GiftCodeAmount >= order.TotalPrice)
                    {
                        // eger ki kuponu totalprice dan yuksek ise sadece kuponla islem yapılabilir. kontrol amaclı ifonlykupon u 1 esitliyorum.
                        order.TotalPrice = 0;
                        ifonlykupontenough = 1;

                    }
                    else
                    {
                        order.TotalPrice = order.TotalPrice - user.GiftCodeAmount;
                    }
                    // ve kullanıcının kuponu kullanılınca kupon ile ilgili alanlarını siliyorum aktif bir kuponu bulunmamıs olucak

                    user.GiftCodeAmount = 0;
                    user.GiftCode = "";
                    user.GiftCodeIsValid = 0;
                    user.GiftCodeExpireDate = DateTime.UtcNow;

                }

               if(ifonlykupontenough== 0)
                {
                    // eger sadece kupon ile odeme yeterli degil ise puan kullanımına giricek.
                    if (user.point > 0)
                    {
                        if (user.point >= order.TotalPrice)
                        {
                            // aynı sekilde puanları total sepetden fazla ise sadece puanlar ile odeme yapılabilir. ve yahut kupon ile bir kısmı odendikten sonra kalan ücret puandan kucukse de burda ki if bloguna 
                            // girilir.
                            // burda da kontrol amaclı puan kontrolu icin ifonlypoint = 1 esitliyorum.
                            user.point = user.point - order.TotalPrice;
                            order.UsedPoint = order.TotalPrice;
                            order.TotalPrice = 0;
                            ifonlypointenough = 1;
                        }
                        else
                        {
                            // puanlarımız toplam tutardan buyuk degilse toplam tutardan puanlar kullanılarak dusulur.
                            order.TotalPrice = order.TotalPrice - user.point;
                            order.UsedPoint = user.point;


                            user.point = 0;
                        }



                    }
                }
               
               
               // bu if blogunda tüm kupon ile odeme yeterli degil ise tüm puan ile de odeme yeterli degil ise ve yahut ikisinin kullanımında hala toplam tutar odenememis ise bu
               // if bloguna gireriz. zaten kontrollerin ikisi de 0 ise girilmekte ikisinin sıfır olması demek kupon ve puan kısımındaki ilk if lere girememiş olması bu durumda totalprice 
               // hala >0 demektir.
               
                if (ifonlykupontenough == 0 && ifonlypointenough == 0)
                {


                    if (user.balance >= order.TotalPrice)
                    {
                        user.balance = user.balance - order.TotalPrice;
                    }
                    else
                    {
                        // anlık kredi kartı odemesi almıyorum. kullanıcının cuzdanına gidip kredi kartından para yuklemesi gerekiyor sadece cüzdan dan ilerleyen bir seneryo yaptım.
                        return "your balance is not enough for payment you should add money from your creditCard to your balance";
                    }

               
           
                        foreach (var item in productItems)
                        {
                             // burda ise odenilen sepet tutarına gore bir sonraki alısveris icin kazanlıcak puan miktari hesaplanıyor. zaten farkedildigi uzere kupon ve puanlar yeterli ise yukardaki if bloga
                             // giremediği için herhangi bir puan kazanımı yok.
                            decimal productpoint = (item.Product.PointPercent * item.Product.Price * item.Quantity) / 100;
                            if (productpoint > item.Product.MaxPoint)
                            {

                                totalpointforthisorder += (item.Product.MaxPoint * item.Quantity);

                            }
                            else
                            {
                                totalpointforthisorder += productpoint;

                            }

                        }
                        // burda ise isledigim seneryo su sekilde eger toplam sepet tutarının tamamını oducek also kaç puan kazanıcaktı ? ancak kupon veyahut puanlar kullanıldı ise geriye kalan 
                        // tutar icin ne kadar kazanır. toplam sepet tutarı 100 diyelim ancak ben 50 lirayı kupondan puandan ve yahut herhangi birinden karsıladım.
                        // 100 lira icin 20 puan kazanıcaksa ornek 50 lira ödeme yapacagı icin (kalan sepet tutarı icin ne kadar kazanır ?) sonuc olarak kullanıcı 10 puan kazanmıs oluyor verdigim seneryoya göre

                             earnpoint =(order.TotalPrice * totalpointforthisorder) / earnpoint;

                        user.point = earnpoint;
                  


                }
              
                order.isValid = 0;
                order.isCompletedSuccesfully = 1;

                var result = _uof.UserRepository.Upsert(user);
                if (result != null)
                {
                    await _uof.OrderRepository.Upsert(order);
                }

            

                await _uof.UserRepository.Upsert(user);


                await _uof.CompleteAsync();

                return "payment was succesful! you can control your order from previousOrdersReport";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{paymentservice} payment method error", typeof(IPaymentService));
                throw new Exception(ex.Message);
            }


        }
    }
}
