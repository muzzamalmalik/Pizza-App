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

        [HttpGet("GetAllItem/{Categoryid}")]
        public async Task<IActionResult> GetAllItem(int Categoryid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllItem(Categoryid);

            return Ok(_response);
        }

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

    }
}
