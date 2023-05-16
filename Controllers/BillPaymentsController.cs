using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    [Authorize]
    public class BillPaymentsController : BaseApiController
    {
        private readonly IBillPaymentsRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public BillPaymentsController(IBillPaymentsRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddBillPayments")]
        public async Task<IActionResult> AddBillPayments([FromForm] AddBillPaymentsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddBillPayments(model);

            return Ok(_response);
        }

        [HttpGet("GetAllBillPayments/{CompanyId}")]
        public async Task<IActionResult> GetAllBillPayments(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllBillPayments(CompanyId);

            return Ok(_response);
        }

        [HttpGet("GetBillPaymentsById/{id}")]
        public async Task<IActionResult> GetBillPaymentsById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetBillPaymentsById(id);

            return Ok(_response);
        }
    }
}
