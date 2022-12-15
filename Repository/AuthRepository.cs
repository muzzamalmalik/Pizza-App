using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Context;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using PizzaOrder.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PizzaOrder.Repository
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public AuthRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> Login(UserForLoginDto model)
        {
           
           ServiceResponse<object> serviceResponse = new();
            var ObjUser = await 
                            ( from c in _context.Users where 
                             c.ContactNumber.ToLower() == model.ContactNumber.ToLower().Trim()
                             && c.Active == true
                             && (model.UserTypeId ==(int) Helpers.Enums.UserTypeId.Admin  ? c.UserTypeId == model.UserTypeId : true)


                  select new LoginUserDto
                  {
                      Id = c.Id,
                      FullName = c.FullName,
                      Email = c.Email,
                      ContactNumber = c.ContactNumber,
                      UserTypeId = c.UserTypeId != 0? c.UserTypeId: _context.Users.FirstOrDefault(x => x.Id == c.Id).UserTypeId,
                      PasswordHash = c.PasswordHash,
                      PasswordSalt = c.PasswordSalt,
                      LastActive = DateTime.UtcNow,
                      CompanyId=c.CompanyId,
                      VerificationCode= 123456,

                  }).FirstOrDefaultAsync();

            //var userId = _context.Users.Where(x => x.FullName.ToLower() == model.FullName.ToLower());
            if (ObjUser != null)
            {
                {
                    
                }
            }
           
            if (ObjUser == null)
                return null;

            if (!Seed.VerifyPasswordHash(model.Password.Trim(), ObjUser.PasswordHash, ObjUser.PasswordSalt))
                return null;


            serviceResponse.Data = ObjUser;
            serviceResponse.Message = "Successfully loged in";
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> Logout()
        {
            var user = await _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefaultAsync();

            //user.LastActive = null;
            //user.LastLogout = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;
        }

        //public async Task<ServiceResponse<object>> LoginDuringRegister(UserForRegisterDto model)
        //{
        //    var objUser = await (from u in _context.Users
        //                         where u.FullName == model.FullName.Trim()
        //                         || u.Email == model.Email.Trim()
        //                         || u.ContactNumber == model.ContactNumber.Trim()

        //                         select new
        //                         {
        //                             id = u.Id,
        //                             FullName = u.FullName,
        //                             Email = u.Email,
        //                             ContactNumber = u.ContactNumber,
        //                         }).FirstOrDefaultAsync();

        //    if (objUser != null)
        //    {
        //        byte[] passwordHash, passwordSalt;
        //        Seed.CreatePasswordHash(model.ContactNumber, out passwordHash, out passwordSalt);


        //        _context.Users = passwordHash;
        //        userToCreate.PasswordSalt = passwordSalt;
        //    }
        //    else
        //    {
        //        var userToCreate = new User
        //        {
        //            FullName = model.FullName,
        //            UserTypeId = model.UserTypeId.ToNotNull_Int(),
        //            Email = model.Email,
        //            Active = true,
        //            ContactNumber = model.ContactNumber,
        //            CompanyId = model.CompanyId,
        //        };
                

        //        if (model.FullName != null && model.UserTypeId != null)
        //        {
        //            await _context.Users.AddAsync(userToCreate);
        //            await _context.SaveChangesAsync();
        //            _serviceResponse.Success = true;
        //            _serviceResponse.Message = CustomMessage.Added;
        //        }
        //        else
        //        {
        //            _serviceResponse.Success = false;
        //            _serviceResponse.Message = CustomMessage.Invalid;

        //        }

        //    }
        //    return _serviceResponse;
        //}

        public async Task<ServiceResponse<object>> Register(UserForRegisterDto model)
        {
                var objUser = await (from u in _context.Users
                                     where u.ContactNumber == model.ContactNumber.Trim()

                                     select new
                                     {
                                         id = u.Id,
                                         FullName = u.FullName,
                                         ContactNumber = u.ContactNumber,
                                     }).FirstOrDefaultAsync();

                if (objUser != null)
                {
                    if (objUser.ContactNumber.Length > 0 && model.ContactNumber.Trim() == objUser.ContactNumber)
                    {
                        _serviceResponse.Message = CustomMessage.PhoneAlreadyExist;
                        _serviceResponse.Data = objUser.ContactNumber;
                    }
                    //else if (objUser.Email.Length > 0 && model.Email.Trim() == objUser.Email)
                    //{
                    //    _serviceResponse.Message = CustomMessage.EmailAlreadyExist;
                    //    _serviceResponse.Data = objUser.Email;
                    //}


                    _serviceResponse.Success = false;
                }
                else
                {
                    //if (model.ImageData != null && model.ImageData.Length > 0)
                    //{
                    //    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "UserProfile");
                    //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageData.FileName);
                    //    var fullPath = Path.Combine(pathToSave);
                    //    model.FilePath = "UserProfile";
                    //    model.FileName = fileName;
                    //    if (!Directory.Exists(pathToSave))
                    //    {
                    //        Directory.CreateDirectory(pathToSave);
                    //    }
                    //    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "UserProfile", fileName);
                    //    //string pathString = filePath.LastIndexOf("/") + 1;

                    //    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    //    {
                    //        await model.ImageData.CopyToAsync(stream);
                    //    }
                    //}
                    var userToCreate = new User
                    {

                        UserName = model.FullName + "-" ,
                        FullName = model.FullName,
                        UserTypeId = model.UserTypeId.ToNotNull_Int(),
                        Email = model.Email,
                        //Gender = model.Gender,
                        Active = true,
                        //CreatedDateTime = DateTime.UtcNow,
                        ContactNumber = model.ContactNumber,
                        Address = model.Address,
                        CompanyId = model.CompanyId,
                        //UserDesignation=model.UserDesignation,
                        //FileName = model.FileName,
                        //FilePath = model.FilePath,
                        VerifyCode=123456,

                    };
                    byte[] passwordHash, passwordSalt;
                    Seed.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

                    userToCreate.PasswordHash = passwordHash;
                    userToCreate.PasswordSalt = passwordSalt;
                    if (model.FullName != null && model.UserTypeId != null)
                    {
                        await _context.Users.AddAsync(userToCreate);
                        await _context.SaveChangesAsync();
                        _serviceResponse.Data = model;
                        _serviceResponse.Success = true;
                        _serviceResponse.Message = "Registered Successfully";
                    }
                    else
                    {
                        _serviceResponse.Success = false;
                        _serviceResponse.Message = CustomMessage.Invalid;

                    }

                }

                return _serviceResponse;
           
        }

        public async Task<ServiceResponse<object>> VerifyUser(VerifyUserDto model)
        {
            var objUser = await (from u in _context.Users
                                 where u.ContactNumber == model.ContactNumber.Trim()

                                 select new
                                 {
                                     id = u.Id,
                                     FullName = u.FullName,
                                     Email = u.Email,
                                     ContactNumber = u.ContactNumber,
                                     CompanyId = u.CompanyId,
                                     UserTypeId = u.UserTypeId,
                                 }).FirstOrDefaultAsync();
            if(objUser != null)
            {
                if (objUser.ContactNumber.Length > 0 && model.ContactNumber.Trim() == objUser.ContactNumber)
                {
                    _serviceResponse.Message = CustomMessage.PhoneAlreadyExist;
                }
                //else if (objUser.Email.Length > 0 && model.Email.Trim() == objUser.Email)
                //{
                //    _serviceResponse.Message = CustomMessage.EmailAlreadyExist;
                //}

                //Random generator = new Random();
                //String Vcode = generator.Next(0, 1000000).ToString("D6");

                //var userToCreate = new VerifyUserReturnObjDto
                //{
                //    FullName = objUser.FullName,
                //    Email = objUser.Email,
                //    CompanyId = objUser.CompanyId,
                //    UserTypeId = objUser.UserTypeId,
                //    ContactNumber = objUser.ContactNumber,
                //    VerifyCode = Vcode,

                //};

                _serviceResponse.Success = true;
                _serviceResponse.Message = "Number Already Exists, Please Login!";
                _serviceResponse.Data = model.ContactNumber;
            }
            else
            {
                Random generator = new Random();
                String Vcode = generator.Next(0, 1000000).ToString("D6");

                var userToCreate = new VerifyUserReturnObjDto
                {
                    //FullName = model.FullName,
                    //Email = model.Email,
                    ContactNumber = model.ContactNumber,
                    //CompanyId = model.CompanyId,
                    //UserTypeId = model.UserTypeId.ToNotNull_Int(),
                    VerifyCode = Vcode,

                };

                _serviceResponse.Data = userToCreate;
                _serviceResponse.Success = true;
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditUser(int id,UserForEditDto model)
        {
            var objEditUser = await _context.Users.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objEditUser != null)
            {
                if (model.ImageData != null && model.ImageData.Length > 0)
                {
                    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "UserProfile");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageData.FileName);
                    var fullPath = Path.Combine(pathToSave);
                    model.FilePath = "UserProfile";
                    model.FileName = fileName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "UserProfile", fileName);
                    //string pathString = filePath.LastIndexOf("/") + 1;

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await model.ImageData.CopyToAsync(stream);
                    }
                }
                objEditUser.UserName = model.Username;
                objEditUser.FullName = model.FullName.IsNullOrEmpty() ? model.Username : model.FullName;
                objEditUser.Email = model.Email;
                objEditUser.CellPhone = model.CellPhone;
                //objEditUser.UserDesignation = model.UserDesignation;
                objEditUser.FileName = model.FileName;
                objEditUser.FilePath = model.FilePath;
                objEditUser.CompanyId = _LoggedIn_CompanyId;
                objEditUser.UserTypeId = _LoggedIn_UserTypeId;

                _context.Users.Update(objEditUser);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;

            }

                else
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                }
                return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> EditUserImagebyApp(int id, UserForEditDtoAdd model)
        {
            var objEditUser = await _context.Users.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objEditUser != null)
            {
                if (model.ImageData != null && model.ImageData.Length > 0)
                {
                    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "UserProfile");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageData.FileName);
                    var fullPath = Path.Combine(pathToSave);
                    model.FilePath = "UserProfile";
                    model.FileName = fileName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "UserProfile", fileName);
                    //string pathString = filePath.LastIndexOf("/") + 1;

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await model.ImageData.CopyToAsync(stream);
                    }
                }
           
                objEditUser.CompanyId = _LoggedIn_CompanyId;
                objEditUser.UserTypeId = _LoggedIn_UserTypeId;

                _context.Users.Update(objEditUser);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;

            }

            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetProfileData()
        {
            try
            {
                var objProfileData = await _context.Users.FirstOrDefaultAsync(m => m.Id == _LoggedIn_UserID);

                if (objProfileData != null)
                {
                    var data = new ProfileDataDto
                    {
                        Id = _LoggedIn_UserID,
                        UserName = objProfileData.UserName,
                        //Email = objProfileData.Email,
                        FullName = objProfileData.FullName,
                        ContactNumber = objProfileData.ContactNumber,
                        UserTypeId = objProfileData.UserTypeId,
                        CompanyId = objProfileData.CompanyId,
                        Address = objProfileData.Address,
                        FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + objProfileData.FilePath + '/' + objProfileData.FileName,
                    };

                    _serviceResponse.Data = objProfileData;
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = "Record Found";
                }
                else
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllUsers()
        {
            try
            {
                var list = await (from m in _context.Users
                                  where m.Id == _LoggedIn_UserID

                                  select new GetAllUsersDto
                                  {
                                      Id = m.Id,
                                      CompanyId = m.CompanyId,
                                      UserName = m.UserName,
                                      FullName  = m.FullName,
                                      ContactNumber= m.ContactNumber,
                                      UserTypeId= m.UserTypeId,
                                      Address= m.Address,
                                      FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
                                  }).ToListAsync();

                if (list.Count > 0)
                {
                    _serviceResponse.Data = list[0];
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = "Record Found";
                }
                else
                {
                    _serviceResponse.Data = null;
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            return _serviceResponse;
        }


        public async Task<bool> UserExists(string userName)
        {
            if (await _context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower() && x.UserTypeId == _LoggedIn_UserTypeId))
                return true;
            return false;
        }

    }
}
