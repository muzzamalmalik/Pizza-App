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

        [HttpPut("EditItem/{ImageId}")]
        public async Task<IActionResult> EditSlideShow(int ImageId, [FromForm]  SlideShowEditDto model
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditSlideShow(ImageId, model);

            return Ok(_response);
        }
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpGet("GetSliderDetailById/{ImageId}")]
        public async Task<IActionResult> GetSliderDetailById(int ImageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSliderDetailById(ImageId);

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
        [AllowAnonymous]
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
        [HttpDelete("DeleteSliderById/{id}")]
        public async Task<IActionResult> DeleteSliderById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteSliderById(id);

            return Ok(_response);
        }
    }
}
