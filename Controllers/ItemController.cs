using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Threading.Tasks;


namespace PizzaOrder.Controllers
{
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

        [HttpGet("GetAllItem/{CompanyId}/{Categoryid}/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllItem(int CompanyId, int Categoryid, int page, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllItem(CompanyId,Categoryid,page,pageSize);

            return Ok(_response);
        }

        [HttpGet("GetItemDetailsById")]
        public async Task<IActionResult> GetItemDetailsById(int id, int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetItemDetailsById(id, CompanyId);

            return Ok(_response);
        }

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
        [HttpGet("GetItemById")]
        public async Task<IActionResult> GetItemById(int id, int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetItemById(id, CompanyId);

            return Ok(_response);
        }

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


    }
}
