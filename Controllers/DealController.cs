using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    [Authorize(Roles = AppRoles.Admin_Only)]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [HttpPost("AddDealData")]
        public async Task<IActionResult> AddDealData([FromForm] AddDealDataDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddDealData(model);

            return Ok(_response);
        }
        [HttpPut("EditDealData/{id}")]
        public async Task<IActionResult> EditDealData(int id, [FromForm] EditDealDataDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditDealData(id, model);

            return Ok(_response);
        }
        [HttpDelete("DeleteDealById/{id}")]
        public async Task<IActionResult> DeleteDealById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteDealById(id);

            return Ok(_response);
        }
        [HttpGet("GetNewDealDetailsById/{id}")]
        public async Task<IActionResult> GetNewDealDetailsById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetNewDealDetailsById(id);

            return Ok(_response);
        }
    }
}
