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
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromForm] AddUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddUser(model);

            return Ok(_response);
        }
        [HttpPut("EditUsers/{id}")]
        public async Task<IActionResult> EditUsers(int id,[FromForm] EditUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditUser(id, model);

            return Ok(_response);
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllUsers();

            return Ok(_response);
        }
        [HttpGet("GetUsersRoleList")]
        public async Task<IActionResult> GetUsersRoleList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetUserRoleList();

            return Ok(_response);
        }
        [HttpGet("GetUserDetailById/{id}")]
        public async Task<IActionResult> GetUserDetailById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetUserDetailById(id);

            return Ok(_response);
        }
        [HttpDelete("DeleteUserById/{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteUser(id);

            return Ok(_response);
        }
    }
}
