using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
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

        [HttpGet("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllOrder();

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

        [HttpPost("AddCheckOutDetails")]
        public async Task<IActionResult> AddCheckOutDetails(AddCheckOutDetailsDto dtoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddCheckOutDetails(dtoData);

            return Ok(_response);
        }

        [HttpGet("GetPaymentType/{id}")]
        public async Task<IActionResult> GetPaymentType(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetPaymentType(id);

            return Ok(_response);
        }
    }
}
