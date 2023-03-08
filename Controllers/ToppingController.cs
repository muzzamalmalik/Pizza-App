using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    public class ToppingController : BaseApiController
    {
        private readonly IToppingRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ToppingController(IToppingRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddTopping")]
        public async Task<IActionResult> AddTopping(AddToppingDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddTopping(model);

            return Ok(_response);
        }

        [HttpPut("EditTopping/{id}")]
        public async Task<IActionResult> EditTopping(int id, EditToppingDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditTopping(id, model);

            return Ok(_response);
        }

        [HttpGet("GetAllTopping/{CompanyId}")]
        public async Task<IActionResult> GetAllTopping(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllTopping(CompanyId);

            return Ok(_response);
        }
        [HttpPost("AddNewTopping")]
        public async Task<IActionResult> AddNewTopping(AddNewToppingDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddNewTopping(model);

            return Ok(_response);
        }

        [HttpPut("EditMewTopping/{id}")]
        public async Task<IActionResult> EditNewTopping(int id, EditNewToppingDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditNewTopping(id, model);

            return Ok(_response);
        }

        [HttpGet("GetAllNewTopping/{ComapnyId}")]
        public async Task<IActionResult> GetAllNewTopping(int ComapnyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllNewTopping(ComapnyId);

            return Ok(_response);
        }
        [HttpGet("GetAllToppingById/{Id}")]
        public async Task<IActionResult> GetAllToppingById(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllToppingById(Id);

            return Ok(_response);
        }
    }

}
