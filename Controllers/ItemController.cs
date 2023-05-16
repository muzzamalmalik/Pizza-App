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
    [Authorize(Roles = AppRoles.Admin_Only)]
    public class ItemController : BaseApiController
    {

        private readonly IItemRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ItemController(IItemRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;

        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem([FromForm] AddItemDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddItem(model);

            return Ok(_response);
        }


        [HttpPut("EditItem/{id}")]
        public async Task<IActionResult> EditItem(int id, [FromForm] EditItemDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditItem(id, model);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetAllItem/{Categoryid}/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllItem(int Categoryid, int page, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllItem(Categoryid,page,pageSize);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetItemDetailsById/{id}")]
        public async Task<IActionResult> GetItemDetailsById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetItemDetailsById(id);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpPost("GetAllItemByWord")]
        public async Task<IActionResult> GetAllItemByWord([FromForm] GetItemsBySearchFields dtoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllItemByWord(dtoData);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetAllItems/{CompanyId}")]
        public async Task<IActionResult> GetAllItems(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllItems(CompanyId);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetItemById/{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetItemById(id);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpPost("GetItemSearchbylocation")]
        public async Task<IActionResult> GetItemSearchbylocation([FromForm] ItemSearchbylocationDto dtoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetItemSearchbylocation(dtoData);

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpPost("GetItemsByCategoryandPrice")]
        public async Task<IActionResult> GetItemsByCategoryandPrice(ItemsByCategoryandPriceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetItemsByCategoryandPrice(dto);

            return Ok(_response);
        }
        [HttpDelete("DeleteItemById/{id}")]
        public async Task<IActionResult> DeleteItemById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteItemById(id);

            return Ok(_response);
        }
        
    }
}
