using AutoMapper;
using BLL.IConfiguration;
using DAL;
using DAL.DTOs.Requests;
using DAL.DTOs.Responses;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GiftCardService : IGiftCardService
    {
        private readonly IUnitOfWork _uof;
        private readonly ApiDbContext _dbcontext;

        private readonly IMapper _mapper;
        protected readonly ILogger<GiftCardService> _logger;
        public GiftCardService(IUnitOfWork uof, IHttpContextAccessor httpContextAccessor, ApiDbContext db, IMapper mapper, ILogger<GiftCardService> logger, ApiDbContext dbcontext)
        {
            _uof = uof;
            
            _mapper = mapper;
            _logger = logger;
            _dbcontext = dbcontext;

        }

        public async Task<GiftCardResponse> CreateGiftCards(GiftCardRequest giftCardRequest)
        {
            //kullanıcılar için kupon oluşturdugum methoddur. kuponları ayrı bir tabloda tutmayı tercih etmedim. user da kolon olarak tutarak işlemleri yaptım. kuponlar kullanıldıgı zaman isvalid 0 a cekildigi icin
            // otomatik olarak user tablosundan kuponla iligli alanlar siliniyor.ve kullanıcak herhangi bir kuponu olmamış olucak.
            try
            {
                var user = _uof.UserRepository.GetByEmailAddress(giftCardRequest.email);

                Random random = new Random();
                int random_number = random.Next(100000, 1000000);
                user.Result.GiftCode = random_number.ToString();
                user.Result.GiftCodeAmount = giftCardRequest.GiftCodeAmount;
                user.Result.GiftCodeExpireDate = DateTime.UtcNow.AddDays(giftCardRequest.activedays);
                user.Result.GiftCodeIsValid = 1;
                await _uof.UserRepository.Upsert(user.Result);


                await _uof.CompleteAsync();
                var mapper = _mapper.Map<User, GiftCardResponse>(user.Result);
                return mapper;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{giftcardservice} creategiftcard method error", typeof(IGiftCardService));
                throw new Exception(ex.Message);
            }

        }

        public List<GiftCardResponse> GiftCardsForAllUsers()
        {
            // geçerli olarak bulunan tüm kuponlar getiriliyor.
            var users = _dbcontext.User.Where(x => x.GiftCodeIsValid ==1).ToList();

            var mapper = _mapper.Map<List<User>, List<GiftCardResponse>>(users);

            return mapper;

        }

        public async Task<bool> DeleteGiftCards(string email)
        {
            try
            {
                var user = _uof.UserRepository.GetByEmailAddress(email);

                user.Result.GiftCode = "";
                user.Result.GiftCodeAmount = 0;
                user.Result.GiftCodeExpireDate = DateTime.UtcNow;
                user.Result.GiftCodeIsValid = 0;
                await _uof.UserRepository.Upsert(user.Result);


                await _uof.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{giftcardservice} deletegiftcards method error", typeof(IGiftCardService));
                throw new Exception(ex.Message);
            }


        }
    }
}
