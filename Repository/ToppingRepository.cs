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
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PizzaOrder.Repository
{
    public class ToppingRepository : BaseRepository, IToppingRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public ToppingRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddTopping(AddToppingDto dtoData)
        {

            if (dtoData != null)
            {
                var ToppingToCreate = new Topping
                {
                    Name = dtoData.Name,
                    Price = dtoData.Price,
                    //CategoryId = dtoData.CategoryId,
                    ItemId = dtoData.ItemId,
                    ItemSizeId = dtoData.ItemSizeId,
                    CompanyId = dtoData.CompanyId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.Toppings.AddAsync(ToppingToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = ToppingToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }
        
            
                

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditTopping(int id, EditToppingDto dtoData)
        {
            var objtopping = await _context.Toppings.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objtopping != null)
            {
                objtopping.Name = dtoData.Name;
                objtopping.Price = dtoData.Price;
                //objtopping.CategoryId = dtoData.CategoryId;
                objtopping.ItemId = dtoData.ItemId;
                objtopping.ItemSizeId = dtoData.ItemSizeId;
                objtopping.CompanyId = dtoData.CompanyId;
                objtopping.UpdateById = _LoggedIn_UserID;
                objtopping.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                _context.Toppings.Update(objtopping);
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

        public async Task<ServiceResponse<object>> GetAllTopping(int CompanyId)
        {
            var list = await (from m in _context.Toppings
                              where m.CompanyId == CompanyId

                              select new GetAllToppingDto
                              {
                                  Id = m.Id,
                                  CompanyId = m.CompanyId,
                                  Name = m.Name,
                                  //BCategoryId = m.CategoryId,
                                  Price = m.Price,
                                  ItemSizeId = m.ItemSizeId,
                                  ItemId = m.ItemId,
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
        public async Task<ServiceResponse<object>> AddNewTopping(AddNewToppingDto dtoData)
        {

            if (dtoData != null)
            {
                var ToppingToCreate = new Topping
                {
                    Name = dtoData.Name,
                    Price = dtoData.Price,
                    //CategoryId = dtoData.CategoryId,
                    // ItemId = dtoData.ItemId,
                    // ItemSizeId = dtoData.ItemSizeId,
                    CompanyId = dtoData.CompanyId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.Toppings.AddAsync(ToppingToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = ToppingToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }




            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditNewTopping(int id, EditNewToppingDto dtoData)
        {
            var objtopping = await _context.Toppings.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objtopping != null)
            {
                objtopping.Name = dtoData.Name;
                objtopping.Price = dtoData.Price;
                //objtopping.CategoryId = dtoData.CategoryId;
                //objtopping.ItemId = dtoData.ItemId;
                //objtopping.ItemSizeId = dtoData.ItemSizeId;
                objtopping.CompanyId = dtoData.CompanyId;
                objtopping.UpdateById = _LoggedIn_UserID;
                objtopping.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                _context.Toppings.Update(objtopping);
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

        public async Task<ServiceResponse<object>> GetAllNewTopping(int CompanyId)
        {
            var list = await (from m in _context.Toppings
                              where m.CompanyId == CompanyId

                              select new GetAllNewToppingDto
                              {
                                  Id = m.Id,
                                  CompanyId = m.CompanyId,
                                  Name = m.Name,
                                  //CategoryId = m.CategoryId,
                                  Price = m.Price,
                                  //ItemSizeId = m.ItemSizeId,
                                  // ItemId = m.ItemId,
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
        public async Task<ServiceResponse<object>> GetAllToppingById(int Id)
        {
            var list = await (from m in _context.Toppings
                              where m.Id == Id

                              select new GetAllNewToppingDto
                              {
                                  Id = m.Id,
                                  CompanyId = m.CompanyId,
                                  Name = m.Name,
                                  //CategoryId = m.CategoryId,
                                  Price = m.Price,
                                  //ItemSizeId = m.ItemSizeId,
                                  // ItemId = m.ItemId,
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
