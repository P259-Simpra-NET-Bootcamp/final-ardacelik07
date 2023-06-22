using AutoMapper;
using BLL.IConfiguration;
using BLL.IRepositories;
using BLL.Services;
using DAL;
using DAL.DTOs.Dtos;
using DAL.DTOs.Responses;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _uof;


        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApiDbContext _db;

        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;


        private string UserId = "";

        public OrderController(ILogger<UserController> logger, IUnitOfWork uof, IHttpContextAccessor httpContextAccessor, ApiDbContext db,
            IMapper mapper, IPaymentService paymentService)
        {
            _logger = logger;
            _uof = uof;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _paymentService = paymentService;



        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            var email = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _uof.UserRepository.GetByEmailAddress(email);
            
           if(user == null)
            {
                return BadRequest("you should login first");
            }

            var order = _uof.OrderRepository.CreateOrder(user.Id);
            if(order == null)
            {
                return BadRequest("you already have an valid order");
            }

            var mapper = _mapper.Map<Order,OrderResponse>(order);

            return Ok(mapper);




        }

        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("additemtotheOrder")]
        public async Task<IActionResult> AddItemToTheBasket(OrderDto orderDto)
        {
            var email = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var user =await  _uof.UserRepository.GetByEmailAddress(email);
            if (user == null)
            {
                return BadRequest("you should login first");
            }

            orderDto.email = email;
            orderDto.Id = user.Id;
            var product = _uof.ProductRepository.GetByIdWithInteger(orderDto.ProductId);
            orderDto.price = product.Price;

            var order = _uof.OrderRepository.AddItemToTheOrder(orderDto);
            if (order == null)
            {
                return BadRequest("you should  create a basket first");
            }

            var mapper = _mapper.Map<Order, OrderResponse>(order);

            return Ok(mapper);




        }
        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("GetTheCurrentOrder")]
        public async Task<IActionResult> GetTheCurrentOrder()
        {
            var email = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _uof.UserRepository.GetByEmailAddress(email); 

             if(user == null)
            {
                return BadRequest("you should login first");
            }
            var currentorder = _uof.OrderRepository.GetTheCurrentOrder(user.Id);

            if(currentorder == null)
            {
                return BadRequest("you dont have any current order");
            }    
            var mapper = _mapper.Map<Order, ReportOrderResponse>(currentorder);


            return Ok(mapper);



        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("GetTheUserPreviousOrders")]
        public async Task<IActionResult> GetTheUserPreviousOrders()
        {
            var email = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _uof.UserRepository.GetByEmailAddress(email);

            if (user.Result == null)
            {
                return BadRequest("you should login first");
            }
            var currentorder = _uof.OrderRepository.UserPreviousOrders(user.Result.Id);
            var mapper = _mapper.Map<List<Order>, List<ReportPreviousOrdersResponse>>(currentorder);


            return Ok(mapper);



        }
        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("ComplateOrderPayment")]
        public async Task<IActionResult> ComplateOrder( [FromQuery] string kuponcode)
        {



            var email = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await  _uof.UserRepository.GetByEmailAddress(email);

            //kupon kontrolu
            var kuponcontrol = user.GiftCode.Equals(kuponcode);

           // kullanıcı kupon kodu girmeden islem yapabilmelidir. bu yuzden herhangi bir query girmeden işlem yapabilmek icin kuponcode null mı degil mi ? diye kontrol ediyorum
            if(kuponcode != null)
            {
                // eger kupon kodu girilmiş ise bu kupon kodunun bulunup bulunmadıgını o kullanıcı icin kontrol etmeliyiz.
                if (kuponcontrol == false)
                {
                    return BadRequest("this giftcard already used or the code is not exist");
                }

            }


            if (user == null)
            {
                return BadRequest("you should login first");
            }


        

            var param = new PaymentParams()
            {
                Id = user.Id,
                kupon = kuponcode 
            };
            var paymentcompletedorder = await _paymentService.ComplateOrder(param);
           






            return Ok(paymentcompletedorder);



        }
    }
}
