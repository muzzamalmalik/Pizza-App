﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
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
        public async Task<IActionResult> EditOrderDetail(int id, EditOrderDetailDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditOrderDetail(id, model);

            return Ok(_response);
        }

        [HttpGet("GetAllOrderDetail/{CompanyId}")]
        public async Task<IActionResult> GetAllOrderDetail(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllOrderDetail(CompanyId);

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
        public async Task<IActionResult> DeleteOrderDetailById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteOrderDetailById(id);

            return Ok(_response);
        }
    }
}