using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;
using static PizzaOrder.Helpers.Enums;

namespace PizzaOrder.Controllers
{
    [Authorize(Roles = AppRoles.Admin_Only)]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

       // [Authorize(Roles = "Admin")]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(AddCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddCategory(model);

            return Ok(_response);
        }

        [HttpPut("EditCategory/{id}")]
        public async Task<IActionResult> EditCategory(int id,EditCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditCategory(id,model);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetAllCategories/{CompanyId}")]
        public async Task<IActionResult> GetAllCategories(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllCategories(CompanyId);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetCategoryById/{id}/{CompanyId}")]
        public async Task<IActionResult> GetCategoryById(int id, int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetCategoryById(id, CompanyId);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetCategoryWithItemsList/{size}/{companyId}")]
        public async Task<IActionResult> GetCategoryWithItemsList(int size,int? companyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetCategoryWithItemsList(size, companyId);

            return Ok(_response);
        }
        [HttpDelete("DeleteCategoryById/{id}")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteCategoryById(id);

            return Ok(_response);
        }
    }
}
