using AutoMapper;
using BLL.IConfiguration;
using BLL.IRepositories;
using BLL.Services;
using DAL;
using DAL.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCardController : ControllerBase
    {




        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApiDbContext _db;

        private readonly IMapper _mapper;
        private readonly IGiftCardService _GiftCardService;
        private readonly IUnitOfWork _unitOfWork;

        private string UserId = "";

        public GiftCardController(IHttpContextAccessor httpContextAccessor, ApiDbContext db,
            IMapper mapper,
           IGiftCardService GiftCardService, IUnitOfWork unitOfWork
            )
        {


            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _GiftCardService = GiftCardService;
            _unitOfWork = unitOfWork;




        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [Route("CreateGiftCard")]
        public async Task<IActionResult> CreateGiftCardForAllUsers(GiftCardRequest giftCardRequest)
        {
            var result =await  _GiftCardService.CreateGiftCards(giftCardRequest);

            //kullanıcı icin admin tarafından kupon olusturulur.

            var expiredatestring = result.GiftCodeExpireDate.ToString();
          await _unitOfWork.Mail.SendEmailForGiftCard(result.Email, result.GiftCode, result.GiftCodeAmount.ToString(), expiredatestring);
            if(result != null)
            {
                return Ok("Gift Card created ");
            }
            return BadRequest();

        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("GiftCardsForAllUsers")]
        public async Task<IActionResult> GiftCardsForAllUsers()
        {
            var giftCards = _GiftCardService.GiftCardsForAllUsers();
            if (giftCards != null)
            {
                return Ok(giftCards);
            }
            return BadRequest();

        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        [Route("DeleteGiftCard/{email}")]
        public async Task<IActionResult> DeleteGiftCards(string email)
        {
            var result =await _GiftCardService.DeleteGiftCards(email);
            if (result == true)
            {
                return Ok("gift cards are deleted");
            }
            return BadRequest();

        }
    }
}
