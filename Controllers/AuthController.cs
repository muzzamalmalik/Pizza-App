
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PizzaOrder.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(IAuthRepository repo, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.FullName = model.FullName.ToLower();

            if (await _repo.UserExists(model.FullName))
                return BadRequest(new { message = CustomMessage.UserAlreadyExist });

            _response = await _repo.Register(model);

            return Ok(_response);
        }

        [HttpPost("VerifyUser")]
        public async Task<IActionResult> VerifyUser(VerifyUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.VerifyUser(model);

            return Ok(_response);
        }
  
        [HttpPut("EditUser/{id}")]
        public async Task<IActionResult> EditUser(int id, [FromForm] UserForEditDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.Username = model.Username.ToLower();

           

            _response = await _repo.EditUser(id,model);    

            return Ok(_response);

        }

        [HttpGet("GetProfileData")]
        public async Task<IActionResult> GetProfileData()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetProfileData();

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


        [HttpGet("GetAdminlogoAndBrandLogoData/{CompanyId}")]
        public async Task<IActionResult> GetAdminlogoAndBrandLogoData(int CompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAdminlogoAndBrandLogoData(CompanyId);

            return Ok(_response);
        }

        [HttpPut("EditUserImagebyApp/{id}")]
        public async Task<IActionResult> EditUserImagebyApp(int id, [FromForm] UserForEditDtoAdd model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditUserImagebyApp(id, model);

            return Ok(_response);

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var  userFromRepo = await  _repo.Login(userForLoginDto);

            if (userFromRepo == null)
            {
                _response.Success = false;
                _response.Message = CustomMessage.UserUnAuthorized;
                return Ok(_response);
            }

            LoginUserDto objUserLogin =(LoginUserDto) userFromRepo.Data;

            Claim[] claims = new[]
            {
                new Claim(Enums.ClaimType.UserId.ToString(), objUserLogin.Id.ToString()),
                new Claim(Enums.ClaimType.Name.ToString(), objUserLogin.FullName.ToString()),
                new Claim(Enums.ClaimType.UserTypeId.ToString(), objUserLogin.UserTypeId.ToString()),
                new Claim(Enums.ClaimType.CompanyId.ToString(), objUserLogin.CompanyId.ToString()),
                new Claim(ClaimTypes.Role, objUserLogin.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            _response.Data = new
            {
                loggedInUserId = claims.FirstOrDefault(x => x.Type.Equals(Enums.ClaimType.UserId.ToString())).Value,
                loggedInUserName = claims.FirstOrDefault(x => x.Type.Equals(Enums.ClaimType.Name.ToString())).Value,
                token = tokenHandler.WriteToken(token),
                loggedInUserTypeId = claims.FirstOrDefault(x => x.Type.Equals(Enums.ClaimType.UserTypeId.ToString())).Value,
                CompanyId = claims.FirstOrDefault(x => x.Type.Equals(Enums.ClaimType.CompanyId.ToString())).Value,
                Role = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role.ToString())).Value,
            };
            _response.Success = true;
            _response.Message = "Successfully loged in";
            return Ok(_response);
        }

        [HttpPut("Logout")]
        public async Task<IActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.Logout();

            return Ok(_response);
        }

        [NonAction]
        private FileStreamResult ReadTxtContent(string Path, string fileName)
        {
            if (!System.IO.File.Exists(Path))
            {
                return null;
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(Path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(Path), fileName);

        }
        [NonAction]
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        [NonAction]
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp3", "audio/mp3"},
                {".wav", "audio/wav"}
            };
        }
    }
}
