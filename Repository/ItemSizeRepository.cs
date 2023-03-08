using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Writers;
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
    public class ItemSizeRepository : BaseRepository, IItemSizeRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public ItemSizeRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddItemSize(AddItemSizeDto dtoData)
        {
            var objItemSize = await (from a in _context.ItemSize
                                     where a.SizeDescription == dtoData.SizeDescription.Trim()
                                     && a.ItemId == dtoData.ItemId
                                     && a.CompanyId == Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value)

                                     select new
                                     {
                                         SizeDescription = a.SizeDescription,
                                         Price= a.Price,
                                         ItemId = a.ItemId,
                                         CompanyId = a.CompanyId,
                                     }).FirstOrDefaultAsync();

            if(objItemSize != null)
            {
                if (objItemSize.SizeDescription.Length > 0 && dtoData.SizeDescription.Trim() == objItemSize.SizeDescription)
                {
                    _serviceResponse.Message = "Item Size with this Name ALready Exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = objItemSize.SizeDescription;
            }
            else
            {
                var ItemSizeToCreate = new ItemSize
                {
                    SizeDescription = dtoData.SizeDescription,
                    Price= dtoData.Price,
                    ItemId = dtoData.ItemId,    
                    CompanyId = Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value),
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };
                try 
                {
                    await _context.ItemSize.AddAsync(ItemSizeToCreate);
                    await _context.SaveChangesAsync();
                    //_serviceResponse.Data = ItemSizeToCreate;
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Added;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditItemSize(int id, EditItemSizeDto dtoData)
        {
            var objitemsize = await _context.ItemSize.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (objitemsize == null)
            {
                objitemsize.SizeDescription = dtoData.SizeDescription;
                objitemsize.Price= dtoData.Price;
                objitemsize.ItemId= dtoData.ItemId;
                objitemsize.CompanyId= dtoData.CompanyId;
                objitemsize.UpdateById = _LoggedIn_UserID;
                objitemsize.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow))  ;

                 _context.ItemSize.Update(objitemsize);
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

        public async Task<ServiceResponse<object>> GetAllItemSize()
        {
            var list = await (from a in _context.ItemSize where a.CompanyId == Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value)

                              select new GetAllItemSizeDto
                              {
                                  Id= a.Id,
                                  SizeDescription = a.SizeDescription,
                                  Price = a.Price,
                                  ItemId= a.ItemId,
                                  CompanyId= a.CompanyId,
                                  CreatedById = a.CretedById,
                                  DateCreated = a.DateCreated,
                                  UpdatedById = a.UpdateById,
                                  DateModified = a.DateModified,
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
        public async Task<ServiceResponse<object>> GetItemSizeById(int id)
        {
            var objitemsize = await _context.ItemSize.Where(x => x.Id.Equals(id) && x.CompanyId == Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value)).FirstOrDefaultAsync();
            if (objitemsize != null)
            {
                var data = new GetItemSizeByIdDto
                {
                    Id = objitemsize.Id,
                    SizeDescription = objitemsize.SizeDescription,
                    Price = objitemsize.Price,
                    ItemId = objitemsize.ItemId,
                    CompanyId = objitemsize.CompanyId,
                    CreatedById = objitemsize.CretedById,
                    DateCreated = objitemsize.DateCreated,
                    UpdatedById = objitemsize.UpdateById,
                    DateModified = objitemsize.DateModified,
                };

                _serviceResponse.Data = data;
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

