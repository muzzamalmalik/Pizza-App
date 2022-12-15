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
    public class CrustController : BaseApiController
    {
        private readonly ICrustRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public CrustController(ICrustRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddCrust")]
        public async Task<IActionResult> AddCrust(AddCrustDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddCrust(model);

            return Ok(_response);
        }

        [HttpPut("EditCrust/{id}")]
        public async Task<IActionResult> EditCrust(int id, EditCrustDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditCrust(id, model);

            return Ok(_response);
        }

        [HttpGet("GetAllCrust/{CompanyId}")]
        public async Task<IActionResult> GetAllCrust(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllCrust(CompanyId);

            return Ok(_response);
        }
    }
}
