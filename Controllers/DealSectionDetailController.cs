using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    [Authorize(Roles = AppRoles.Admin_Only)]
    public class DealSectionDetailController : BaseApiController
    {
        private readonly IDealSectionDetailRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public DealSectionDetailController(IDealSectionDetailRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddDealSectionDetail")]
        public async Task<IActionResult> AddDealSectionDetail(AddDealSectionDetailDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddDealSectionDetail(model);

            return Ok(_response);
        }

        [HttpPut("EditDealSectionDetail/{id}")]
        public async Task<IActionResult> EditDealSectionDetail(int id, EditDealSectionDetailDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditDealSectionDetail(id, model);

            return Ok(_response);
        }
        [HttpDelete("DeleteDealSectionDetailById/{id}")]
        public async Task<IActionResult> DeleteDealSectionDetailById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteDealSectionDeatilById(id);

            return Ok(_response);
        }

        //[HttpGet("GetAllDealSectionDetail/{CompanyId}")]
        //public async Task<IActionResult> GetAllDealSectionDetail(int CompanyId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    _response = await _repo.GetAllDealSectionDetail(CompanyId);

        //    return Ok(_response);
        //}
    }
}
