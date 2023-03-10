using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Context;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using PizzaOrder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PizzaOrder.Repository
{
    public class CrustRepository : BaseRepository, ICrustRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public CrustRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddCrust(AddCrustDto dtoData)
        {


            if (dtoData != null)
            {
                var CrustToCreate = new Crust
                {
                    Name = dtoData.Name,
                    //Description = dtoData.Description,
                    Price= dtoData.Price,
                    CompanyId= dtoData.CompanyId,
                    //ItemId= dtoData.ItemId,
                   // ItemSizeId= dtoData.ItemSizeId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.Crusts.AddAsync(CrustToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = CrustToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }
             
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditCrust(int id, EditCrustDto dtoData)
        {
            var objcrust = await _context.Crusts.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objcrust != null)
            {
                objcrust.Name = dtoData.Name;
                //objcrust.Description = dtoData.Description;
                objcrust.Price = dtoData.Price;
                //objcrust.CategoryId = dtoData.CategoryId;
                //objcrust.ItemId = dtoData.ItemId;
               // objcrust.ItemSizeId= dtoData.ItemSizeId;
               //objcrust.CompanyId = dtoData.CompanyId;
                objcrust.UpdateById = _LoggedIn_UserID;
                objcrust.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                _context.Crusts.Update(objcrust);
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

        public async Task<ServiceResponse<object>> GetAllCrust(int CompanyId)
        {
            var list = await (from m in _context.Crusts
                              where m.CompanyId == CompanyId

                              select new GetAllCrustDto
                              {
                                  Id = m.Id,
                                  CompanyId = m.CompanyId,
                                  Name = m.Name,
                                  //Description = m.Description,
                                  //CategoryId = m.CategoryId,
                                  Price= m.Price,
                                 // ItemSizeId= m.ItemSizeId,
                                  //ItemId= m.ItemId,
                                  CreatedById = m.CretedById,
                                  DateCreated = m.DateCreated,
                                  UpdatedById = m.UpdateById,
                                  DateModified = m.DateModified,

                              }).ToListAsync();

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
        public async Task<ServiceResponse<object>> AddNewCrust(AddNewCrustDto dtoData)
        {


            if (dtoData != null)
            {
                var CrustToCreate = new Crust
                {
                    Name = dtoData.Name,
                    //Description = dtoData.Description,
                    Price = dtoData.Price,
                    CompanyId = dtoData.CompanyId,
                    //ItemId= dtoData.ItemId,
                    // ItemSizeId= dtoData.ItemSizeId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.Crusts.AddAsync(CrustToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = CrustToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditNewCrust(int id, EditNewCrustDto dtoData)
        {
            var objcrust = await _context.Crusts.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objcrust != null)
            {
                objcrust.Name = dtoData.Name;
                //objcrust.Description = dtoData.Description;
                objcrust.Price = dtoData.Price;
                //objcrust.CategoryId = dtoData.CategoryId;
                //objcrust.ItemId = dtoData.ItemId;
                // objcrust.ItemSizeId= dtoData.ItemSizeId;
                //objcrust.CompanyId = dtoData.CompanyId;
                objcrust.UpdateById = _LoggedIn_UserID;
                objcrust.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                _context.Crusts.Update(objcrust);
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

        public async Task<ServiceResponse<object>> GetAllCrustbyId(int Id)
        {
            var list = await (from m in _context.Crusts
                              where m.Id == Id

                              select new GetAllNewCrustDto
                              {
                                  Id = m.Id,
                                  CompanyId = m.CompanyId,
                                  Name = m.Name,
                                  //Description = m.Description,
                                  //CategoryId = m.CategoryId,
                                  Price = m.Price,
                                  // ItemSizeId= m.ItemSizeId,
                                  //ItemId= m.ItemId,
                                  CreatedById = m.CretedById,
                                  DateCreated = m.DateCreated,
                                  UpdatedById = m.UpdateById,
                                  DateModified = m.DateModified,

                              }).ToListAsync();

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
        public async Task<ServiceResponse<object>> GetAllNewCrust(int CompanyId)
        {
            var list = await (from m in _context.Crusts
                              where m.CompanyId == CompanyId

                              select new GetAllNewCrustDto
                              {
                                  Id = m.Id,
                                  CompanyId = m.CompanyId,
                                  Name = m.Name,
                                  //Description = m.Description,
                                  //CategoryId = m.CategoryId,
                                  Price = m.Price,
                                  // ItemSizeId= m.ItemSizeId,
                                  //ItemId= m.ItemId,
                                  CreatedById = m.CretedById,
                                  DateCreated = m.DateCreated,
                                  UpdatedById = m.UpdateById,
                                  DateModified = m.DateModified,

                              }).ToListAsync();

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
    }
}
