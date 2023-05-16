using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    [Authorize]
    public class LookUpController : BaseApiController
    {
        private readonly ILookUpRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public LookUpController(ILookUpRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet("GetCategoriesList")]
        public async Task<IActionResult> GetCategoriesList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetCategoriesList();

            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpGet("GetItemsByCategoryId")]
        public async Task<IActionResult> GetItemsByCategoryId(int id=0)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetItemsByCategoryId(id);

            return Ok(_response);
        }
    }
}
