using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    [Authorize(Roles = AppRoles.Admin_Only)]
    public class ReportsController : BaseApiController
    {
        private readonly IReportRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public ReportsController(IReportRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet("GetOrderListForReport")]
        public async Task<IActionResult> GetOrderListForReport(string startDate="",string endDate="")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetOrderListForReport();

            return Ok(_response);
        }
        [HttpGet("GetCustomerWiseOrderReport")]
        public async Task<IActionResult> GetCustomerWiseOrderReport(int custId=0,string startDate="",string endDate="")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetCustomerWiseOrderReport();

            return Ok(_response);
        }
    }
}
