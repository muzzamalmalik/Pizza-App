using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    public class DealSectionController : BaseApiController
    {
        private readonly IDealSectionRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public DealSectionController(IDealSectionRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddDealSection")]
        public async Task<IActionResult> AddDealSection(AddDealSectionDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddDealSection(model);

            return Ok(_response);
        }

        [HttpPut("EditDealSection/{id}")]
        public async Task<IActionResult> EditDealSection(int id, EditDealSectionDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditDealSection(id, model);

            return Ok(_response);
        }

        [HttpGet("GetAllDealSection{CompanyId}")]
        public async Task<IActionResult> GetAllDealSection(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllDealSection(CompanyId);

            return Ok(_response);
        }
    }
}
