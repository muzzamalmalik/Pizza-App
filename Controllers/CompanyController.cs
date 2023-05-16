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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpGet("GetAllCompanyByLatLong")]
        public async Task<IActionResult> GetAllCompanyByLatLong(int Range, double Lat, double Long)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllCompanyByLatLong(Range, Lat, Long);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("SearchCompany/{SearchField}")]
        public async Task<IActionResult> SearchCompany(string SearchField)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.SearchCompany(SearchField);

            return Ok(_response);
        }
        [HttpDelete("DeleteCompanyById/{id}")]
        public async Task<IActionResult> DeleteCompanyById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteCompanyById(id);

            return Ok(_response);
        }
    }
}
