using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Context;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using PizzaOrder.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PizzaOrder.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public UserRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }
        public async Task<ServiceResponse<object>> AddUser(AddUserDto dtoData)
        {
            if (dtoData != null)
            {
                if (dtoData.ImageData != null && dtoData.ImageData.Length > 0)
                {
                    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "UserImages");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                    var fullPath = Path.Combine(pathToSave);
                    dtoData.FilePath = "UserImages";
                    dtoData.FileName = fileName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "UserImages", fileName);
                    //string pathString = filePath.LastIndexOf("/") + 1;

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await dtoData.ImageData.CopyToAsync(stream);
                    }
                }
                var userToCreate = new User
                {
                    UserName = dtoData.FullName,
                    FullName = dtoData.FullName,
                    UserTypeId = dtoData.UserTypeId.ToNotNull_Int(),
                    Email = dtoData.Email,
                    Active = dtoData.Active,
                    ContactNumber = dtoData.ContactNumber,
                    FilePath = dtoData.FilePath,
                    FileName = dtoData.FileName,
                    Address = dtoData.Address,
                    CompanyId = _LoggedIn_CompanyId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),

                };
                
                await _context.Users.AddAsync(userToCreate);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
              
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> EditUser(int Id,EditUserDto dtoData)
        {
            var objUser = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (objUser != null)
            {
                if (dtoData.ImageData != null && dtoData.ImageData.Length > 0)
                {
                    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "UserImages");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                    var fullPath = Path.Combine(pathToSave);
                    dtoData.FilePath = "UserImages";
                    dtoData.FileName = fileName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "UserImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await dtoData.ImageData.CopyToAsync(stream);
                    }

                    objUser.FileName = dtoData.FileName;
                    objUser.FilePath = dtoData.FilePath;
                }
                else
                {
                    objUser.FileName = objUser.FileName;
                    objUser.FilePath = objUser.FilePath;
                }
                objUser.UserName = dtoData.FullName;
                objUser.FullName = dtoData.FullName;
                objUser.UserTypeId = dtoData.UserTypeId.ToNotNull_Int();
                objUser.Email = dtoData.Email;
                objUser.Active = dtoData.Active;
                objUser.ContactNumber = dtoData.ContactNumber;
                objUser.Address = dtoData.Address;
                objUser.CompanyId = dtoData.CompanyId;
                objUser.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));
                objUser.UpdateById = _LoggedIn_UserID;

                _context.Users.Update(objUser);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;

            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetUserDetailById(int Id)
        {
            var list = await (from us in _context.Users
                              where us.Id == Id
                              select new GetAllUsersDetailDto
                              {
                                  Id = us.Id,
                                  FullName = us.FullName,
                                  ContactNumber = us.ContactNumber,
                                  Email = us.Email,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + us.FilePath + '/' + us.FileName,
                                  Address = us.Address,
                                  CompanyId = us.CompanyId,
                                  UserTypeId =us.UserTypeId
                              }).ToListAsync();
            if(list.Count>0)
            {
                _serviceResponse.Data= list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetAllUsers()
        {
            var list = await (from us in _context.Users
                              where us.CompanyId == _LoggedIn_CompanyId
                              select new GetAllUsersDetailDto
                              {
                                  Id = us.Id,
                                  FullName = us.FullName,
                                  ContactNumber = us.ContactNumber,
                                  Email = us.Email,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + us.FilePath + '/' + us.FileName,
                                  Address = us.Address,
                                  CompanyId = us.CompanyId,
                                  Role = ((Helpers.Enums.UserTypeId)us.UserTypeId).ToString()
                              }).ToListAsync();
            if(list.Count>0)
            {
                _serviceResponse.Data= list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetUserRoleList()
        {
            var list = (Enum.GetValues(typeof(Helpers.Enums.UserTypeId)).Cast<Helpers.Enums.UserTypeId>().Select(
               enu => new SelectListItem() { Text = enu.ToString(), Value = ((int)enu).ToString() })).ToList();


            if (list.Count > 0)
            {
                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> DeleteUser(int Id)
        {
            var objUser = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (objUser != null)
            {
                _context.Users.Remove(objUser);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;

            }
            return _serviceResponse;
        }

    }
}
