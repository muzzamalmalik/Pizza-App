using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    public class DealController : BaseApiController
    {
        private readonly IDealRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public DealController(IDealRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddDeal")]
        public async Task<IActionResult> AddDeal([FromForm] AddDealDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddDeal(model);

            return Ok(_response);
        }

        [HttpPut("EditDeal/{id}")]
        public async Task<IActionResult> EditDeal(int id, [FromForm] EditDealDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditDeal(id, model);

            return Ok(_response);
        }

        [HttpGet("GetAllDeal/{CompanyId}")]
        public async Task<IActionResult> GetAllDeal(int CompanyId, int page, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllDeal(CompanyId,page,pageSize);

            return Ok(_response);
        }

        [HttpGet("GetDealDetailsById/{id}")]
        public async Task<IActionResult> GetDealDetailsById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetDealDetailsById(id);

            return Ok(_response);
        }
    }
}
