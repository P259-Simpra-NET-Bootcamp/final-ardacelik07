using AutoMapper;
using BLL.IConfiguration;
using DAL;
using DAL.DTOs.Dtos;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _uof;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApiDbContext _db;
    
        private readonly IMapper _mapper;


        private string UserId = "";

        public CategoryController(ILogger<UserController> logger, IUnitOfWork uof, IHttpContextAccessor httpContextAccessor, ApiDbContext db,
            IMapper mapper)
        {
            _logger = logger;
            _uof = uof;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }

        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<IActionResult> GetAll()
        {
            
            var categories =  _uof.CategoryRepository.GetAllWithIncludes();

            return Ok(categories); 
          


        }

        
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id )
        {
            var category = _uof.CategoryRepository.CategoryGetByIdWithIncludes(id);

            return Ok(category);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete]
        [Route("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
           


            var result =  await _uof.CategoryRepository.DeleteByIdWithInteger(id);

            if (result)
            {
                await _uof.CompleteAsync();
                return Ok();
            }
            else
            {
                return BadRequest("error when deleting category");
            }

         
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CategoryCreate(CategoryDto category)
        {

            var categorycreate = new Category()
            {
                Name = category.Name

            };

           var result = await _uof.CategoryRepository.Add(categorycreate);

            

            if (result)
            {
                await _uof.CompleteAsync();
                return Ok();
            }
            else
            {
                return BadRequest("error when adding category");
            }


        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut]
        [Route("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id,  [FromBody] CategoryDto category)
        {


            var updatedCategory =  _uof.CategoryRepository.GetByIdWithInteger(id);

            if (updatedCategory == null)
            {
                return BadRequest("there is no category");
            }
            
               updatedCategory.Name = category.Name;
            var result =await _uof.CategoryRepository.Upsert(updatedCategory);
            


            



            if (result)
            {
                await _uof.CompleteAsync();
                return Ok();
            }
            else
            {
                return BadRequest("error when adding category");
            }


        }

    }
}
