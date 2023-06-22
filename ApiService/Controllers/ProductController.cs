using AutoMapper;
using BLL.IConfiguration;
using DAL.DTOs.Dtos;
using DAL.Models;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using DAL.DTOs.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _uof;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApiDbContext _db;

        private readonly IMapper _mapper;


        private string UserId = "";

        public ProductController(ILogger<UserController> logger, IUnitOfWork uof, IHttpContextAccessor httpContextAccessor, ApiDbContext db,
            IMapper mapper)
        {
            _logger = logger;
            _uof = uof;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAll()
        {

            var products = _uof.ProductRepository.All();
            var mapper = _mapper.Map<List<Product>, List<ProductDto>>(products);

            return Ok(mapper);



        }

       
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = _uof.ProductRepository.GetByIdWithInteger(id);
            var mapper = _mapper.Map<Product, ProductDto>(product);
            return Ok(mapper);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete]
        [Route("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {

            var result = await _uof.ProductRepository.DeleteByIdWithInteger(id);

            if (result)
            {
                await _uof.CompleteAsync();
                return Ok();
            }
            else
            {
                return BadRequest("error when deleting product");
            }


        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> ProductCreate(ProductDto product)
        {

            var productcreate = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                IsValid = true,
                MaxPoint= product.MaxPoint,
                PointPercent= product.PointPercent,
                CategoryId= product.CategoryId,

            };

            var result = await _uof.ProductRepository.Add(productcreate);



            if (result)
            {
                await _uof.CompleteAsync();
                return Ok();
            }
            else
            {
                return BadRequest("error when adding product");
            }


        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto product)
        {


            var productupdate = _uof.ProductRepository.GetByIdWithInteger(id);

            if (productupdate == null)
            {
                return BadRequest("there is no product");
            }

            productupdate.Name = product.Name;
            productupdate.Description = product.Description;
            productupdate.Price = product.Price;
             
                 productupdate.Price= product.Price;
            productupdate.MaxPoint = product.MaxPoint;
            productupdate.PointPercent = product.PointPercent;
            productupdate.CategoryId = product.CategoryId;  
            
            var result = await _uof.ProductRepository.Upsert(productupdate);







            if (result)
            {
                await _uof.CompleteAsync();
                return Ok();
            }
            else
            {
                return BadRequest("error when adding product");
            }


        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        [Route("StockOperations/{id}")]
        public async Task<IActionResult> StockOperations(int id, [FromQuery] StockControlRequest stock)
        {


            var productupdate = _uof.ProductRepository.GetByIdWithInteger(id);

            if (productupdate == null)
            {
                return BadRequest("there is no product");
            }

           if(productupdate.Stock >= 0  )
            {
               
                var product = _uof.ProductRepository.stockControl(id, stock.action, stock.amount);
                if(product == null)
                {
                    return BadRequest("you can not decrease that much");
                }


                var mapper = _mapper.Map<Product, ProductDto>(product);
                await _uof.CompleteAsync();
                return Ok(mapper);
            }
            else
            {
                return BadRequest("the stock can not be less then zero");
            }

      


        }

        
        [HttpGet]
        [Route("StockControl/{inStock}")]
        public async Task<IActionResult> StockControl(bool inStock)
        {


            var productupdate = _uof.ProductRepository.InStock(inStock);

            if (productupdate.Count == 0)
            {
                return BadRequest("there is no product in stock yet");
            }



               return Ok(productupdate);

        }


      
    }
}
