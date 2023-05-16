using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    [Authorize(Roles = AppRoles.Admin_User)]
    public class OrderDetailController : BaseApiController
    {
        private readonly IOrderDetailRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public OrderDetailController(IOrderDetailRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddOrderDetail")]
        public async Task<IActionResult> AddOrderDetail(AddOrderDetailDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddOrderDetail(model);

            return Ok(_response);
        }

        [HttpPost("AddToCartCall")]
        public async Task<IActionResult> AddToCartCall(AddToCartCallDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddToCartCall(model);

            return Ok(_response);
        }

        [HttpPut("EditOrderDetail/{id}")]
        public async Task<IActionResult> EditOrderDetail(int id, OrderDetailDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditOrderDetail(id, model);

            return Ok(_response);
        }

        [HttpGet("GetAllOrderDetail")]
        public async Task<IActionResult> GetAllOrderDetail()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllOrderDetail();

            return Ok(_response);
        }

        [HttpGet("GetOrderDetailById/{id}")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetOrderDetailById(id);

            return Ok(_response);
        }

        [HttpDelete("DeleteOrderDetailById/{id}")]
        public async Task<IActionResult> DeleteOrderDetailById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteOrderDetailById(id);

            return Ok(_response);
        }
        [HttpDelete("DeleteOrderDetailByOrderDetailId/{id}/{dealId}")]
        public async Task<IActionResult> DeleteOrderDetailByOrderDetailId(int id, int? dealId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteOrderDetailByOrderDetailId(id, dealId);

            return Ok(_response);
        }
        [HttpGet("GetDealItemsListById/{id}/{CategoryId}")]
        public async Task<IActionResult> GetDealItemsListById(int id, int CategoryId = 0)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetDealItemsListById(id, CategoryId);

            return Ok(_response);
        }
    }
}
