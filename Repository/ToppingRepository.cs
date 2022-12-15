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
            var objTopping = await (from u in _context.Toppings
                                  where u.Name == dtoData.Name.Trim()
                                  && u.ItemId == dtoData.ItemId
                                  && u.CompanyId == dtoData.CompanyId

                                  select new
                                  {
                                      Name = u.Name,
                                      CompanyId = u.CompanyId,
                                      Price = u.Price,
                                  }).FirstOrDefaultAsync();

            if (objTopping != null)
            {
                if (objTopping.Name.Length > 0 && dtoData.Name.Trim() == objTopping.Name)
                {
                    _serviceResponse.Message = "Topping with  this Name ALready Exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = objTopping.Name;
            }
            else
            {
                var ToppingToCreate = new Topping
                {
                    Name = dtoData.Name,
                    Price = dtoData.Price,
                    CategoryId = dtoData.CategoryId,
                    ItemId = dtoData.ItemId,
                    ItemSizeId = dtoData.ItemSizeId,
                    CompanyId = dtoData.CompanyId,
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
                objtopping.CategoryId = dtoData.CategoryId;
                objtopping.ItemId = dtoData.ItemId;
                objtopping.ItemSizeId = dtoData.ItemSizeId;
                objtopping.CompanyId = dtoData.CompanyId;

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
                                  CategoryId = m.CategoryId,
                                  Price = m.Price,
                                  ItemSizeId = m.ItemSizeId,
                                  ItemId = m.ItemId,

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
