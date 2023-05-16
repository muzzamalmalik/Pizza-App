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
    [Authorize(Roles = AppRoles.Admin_Only)]
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
        [AllowAnonymous]
        [HttpGet("GetAllCrust")]
        public async Task<IActionResult> GetAllCrust()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllCrust();

            return Ok(_response);
        }
        [HttpPost("AddNewCrust")]
        public async Task<IActionResult> AddCrust(AddNewCrustDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddNewCrust(model);

            return Ok(_response);
        }

        [HttpPut("EditNewCrust/{id}")]
        public async Task<IActionResult> EditNewCrust(int id, EditNewCrustDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditNewCrust(id, model);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetAllNewCrust")]
        public async Task<IActionResult> GetAllNewCrust()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllNewCrust();

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetAllCrustbyId/{Id}")]
        public async Task<IActionResult> GetAllCrustbyId(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllCrustbyId(Id);

            return Ok(_response);
        }
        [HttpDelete("DeleteCrustById/{id}")]
        public async Task<IActionResult> DeleteCrustById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteCrustById(id);

            return Ok(_response);
        }
    }
}
