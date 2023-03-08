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
    public class OrderController : BaseApiController
    {
        private readonly IOrderRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(AddOrderDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddOrder(model);

            return Ok(_response);
        }

        [HttpPut("EditOrder/{id}")]
        public async Task<IActionResult> EditOrder(int id, EditOrderDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditOrder(id, model);

            return Ok(_response);
        }

        //[HttpGet("GetAllOrder/{OrderStatus}")]
        //public async Task<IActionResult> GetAllOrder(int OrderStatus)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    _response = await _repo.GetAllOrder(OrderStatus);

        //    return Ok(_response);
        //}

        [HttpGet("GetOrderById/{id}/{orderStatus}")]
        public async Task<IActionResult> GetOrderById(int id, int orderStatus, int page, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetOrderById(id, orderStatus, page, pageSize);

            return Ok(_response);
        }

        [HttpGet("GetCartList")]
        public async Task<IActionResult> GetCartList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetCartList();

            return Ok(_response);
        }

        [HttpPost("ProcessOrder")]
        public async Task<IActionResult> ProcessOrder(AddProcessOrderDto dtoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.ProcessOrder(dtoData);

            return Ok(_response);
        }

        [HttpGet("GetPaymentType")]
        public async Task<IActionResult> GetPaymentType()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetPaymentType();

            return Ok(_response);
        }


        [HttpGet("GetOrderForRider/{CompanyId}")]
        public async Task<IActionResult> GetOrderForRider(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetOrderForRider(CompanyId);

            return Ok(_response);
        }
        [HttpPut("RiderStuatusUpdate")]
        public async Task<IActionResult> RiderStuatusUpdate(AddProcessOrderDto dtoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.RiderStuatusUpdate(dtoData);

            return Ok(_response);
        }
        [HttpGet("GetOrderByCompany/{CompanyId}/{orderStatus}")]
        public async Task<IActionResult> GetOrderByCompany(int CompanyId, int orderStatus, int page, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetOrderByCompany(CompanyId, orderStatus, page, pageSize);

            return Ok(_response);
        }
    }
}
