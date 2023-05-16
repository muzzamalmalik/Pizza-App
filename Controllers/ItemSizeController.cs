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
    public class ItemSizeController : BaseApiController
    {
        private readonly IItemSizeRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ItemSizeController(IItemSizeRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddItemSize")]
        public async Task<IActionResult> AddItemSize(AddItemSizeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddItemSize(model);

            return Ok(_response);
        }

        [HttpPut("EditItemSize/{id}")]
        public async Task<IActionResult> EditItemSize(int id, EditItemSizeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditItemSize(id, model);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetAllItemSize")]
        public async Task<IActionResult> GetAllItemSize()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllItemSize();

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetAllItemSizes")]
        public async Task<IActionResult> GetAllItemSizes()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllItemSizes();

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetItemSizeById/{id}")]
        public async Task<IActionResult> GetItemSizeById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetItemSizeById(id);

            return Ok(_response);
        }
        [HttpDelete("DeleteItemSizeById/{id}")]
        public async Task<IActionResult> DeleteItemSizeById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteItemSizeById(id);

            return Ok(_response);
        }
    }
}
