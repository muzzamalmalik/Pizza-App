using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    public class CompanyController : BaseApiController
    {
        private readonly ICompanyRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromForm] AddCompanyDto dtoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddCompany(dtoData);

            return Ok(_response);
        }

        [HttpPut("EditCompany/{id}")]
        public async Task<IActionResult> EditCompany(int id, [FromForm] EditCompanyDto dtoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditCompany(id, dtoData);

            return Ok(_response);
        }

        [HttpGet("GetAllCompany")]
        public async Task<IActionResult> GetAllCompany()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllCompany();

            return Ok(_response);
        }

        [HttpGet("GetCompanyById/{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetCompanyById(id);

            return Ok(_response);
        }

    }
}
