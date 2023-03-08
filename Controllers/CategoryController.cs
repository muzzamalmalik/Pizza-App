using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
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

        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id, int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetCategoryById(id, CompanyId);

            return Ok(_response);
        }
    }
}
