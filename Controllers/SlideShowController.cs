using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
   
    public class SlideShowController : BaseApiController
    {
        private readonly ISlideShowRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public SlideShowController(ISlideShowRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddSlideShow")]
        public async Task<IActionResult> AddSlideShow([FromForm] SlideShowAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddSlideShow(model);

            return Ok(_response);
        }

        [HttpPut("EditItem/{id}")]
        public async Task<IActionResult> EditSlideShow(int id, int imageid, [FromForm]  SlideShowEditDto model
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditSlideShow(id,imageid, model);

            return Ok(_response);
        }

        [HttpGet("GetAllSlideShows/{CompanyId}")]
        public async Task<IActionResult> GetAllSlideShows(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllSlideShows(CompanyId);

            return Ok(_response);
        }

        [HttpPost("AddFeaturedAds")]
        public async Task<IActionResult> AddFeaturedAds([FromForm] AddFeaturedAdsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddFeaturedAds(model);

            return Ok(_response);
        }
        [HttpGet("GetAllFeaturedAds/{Lat}/{Long}/{Range}")]
        public async Task<IActionResult> GetAllFeaturedAds(double Lat, double Long, int Range)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllFeaturedAds(Lat,Long,Range);

            return Ok(_response);
        }
        [HttpPut("EditFeaturedAds/{id}")]
        public async Task<IActionResult> EditFeaturedAds(int id, [FromForm] EditFeaturedAdsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditFeaturedAds(id, model);

            return Ok(_response);
        }
    }
}
