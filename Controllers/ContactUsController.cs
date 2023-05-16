using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    public class ContactUsController : BaseApiController
    {
        private readonly IContactUsRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ContactUsController(IContactUsRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }
        [HttpPost("AddContactPersonInfo")]
        public async Task<IActionResult> AddContactPersonInfo(AddContactUsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddContactPersonInfo(model);

            return Ok(_response);
        }
        [HttpGet("GetContactPersonsInfoList/{CompanyId}")]
        public async Task<IActionResult> GetContactPersonInfoList(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetContactPersonInfoList(CompanyId);

            return Ok(_response);
        }
    }
}
