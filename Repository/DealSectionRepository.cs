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
    public class DealSectionRepository : BaseRepository, IDealSectionRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public DealSectionRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddDealSection(AddDealSectionDto dtoData)
        {
            if(dtoData != null)
            {
                var DealSectionToCreate = new DealSection
                {
                    DealId = dtoData.DealId,
                    Title = dtoData.Title,
                    Description = dtoData.Description,
                    CategoryId = dtoData.CategoryId,
                    ChooseQuantity = dtoData.ChooseQuantity,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.DealSection.AddAsync(DealSectionToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = DealSectionToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditDealSection(int id, EditDealSectionDto dtoData)
        {
            var objdealsection = await _context.DealSection.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objdealsection != null)
            {
                objdealsection.DealId = dtoData.DealId;
                objdealsection.Title = dtoData.Title;
                objdealsection.Description = dtoData.Description;
                objdealsection.CategoryId = dtoData.CategoryId;
                objdealsection.ChooseQuantity = dtoData.ChooseQuantity;
                objdealsection.UpdateById = _LoggedIn_UserID;
                objdealsection.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                _context.DealSection.Update(objdealsection);
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

        //public async Task<ServiceResponse<object>> GetAllDealSection()
        //{
        //    var list = await (from m in _context.DealSection
        //                      where Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value) == m.

        //                      select new GetAllDealSectionDto
        //                      {
        //                          Id = m.Id,
        //                          DealId = m.DealId,
        //                          Title = m.Title,
        //                          Description = m.Description,
        //                          ChooseQuantity = m.ChooseQuantity,
        //                          CategoryId = m.CategoryId,
        //                          CategoryName = _context.Category.FirstOrDefault(x => x.Id == m.CategoryId).Name,
        //                          CreatedById = m.CretedById,
        //                          DateCreated = m.DateCreated,
        //                          UpdatedById = m.UpdateById,
        //                          DateModified = m.DateModified,
        //                          MultiSelect = m.ChooseQuantity >1 ? true: false,

        //                      }).ToListAsync();

        //    if (list.Count > 0)
        //    {
        //        _serviceResponse.Data = list;
        //        _serviceResponse.Success = true;
        //        _serviceResponse.Message = "Record Found";
        //    }
        //    else
        //    {
        //        _serviceResponse.Data = null;
        //        _serviceResponse.Success = false;
        //        _serviceResponse.Message = CustomMessage.RecordNotFound;
        //    }
        //    return _serviceResponse;
        //}
    }
}
