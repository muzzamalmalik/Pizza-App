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
            var objDealSection = await (from u in _context.DealSection
                                    where  u.CompanyId != dtoData.CompanyId

                                    select new
                                    {
                                        DealId = u.DealId,
                                        ChooseQuantity = u.ChooseQuantity,
                                        CategoryId = u.CategoryId,
                                        CompanyId = u.CompanyId,
                                    }).FirstOrDefaultAsync();

            if (objDealSection != null)
            {
                if (objDealSection.CompanyId > 0 && dtoData.CompanyId != objDealSection.CompanyId)
                {
                    _serviceResponse.Message = "This Company Id not exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = dtoData.CompanyId;
            }
            else
            {
                var DealSectionToCreate = new DealSection
                {
                    DealId = dtoData.DealId,
                    CategoryId = dtoData.CategoryId,
                    ChooseQuantity= dtoData.ChooseQuantity,
                    CompanyId = dtoData.CompanyId,
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
                objdealsection.CategoryId = dtoData.CategoryId;
                objdealsection.ChooseQuantity = dtoData.ChooseQuantity;
                objdealsection.CompanyId = dtoData.CompanyId;

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

        public async Task<ServiceResponse<object>> GetAllDealSection(int CompanyId)
        {
            var list = await (from m in _context.DealSection
                              where m.CompanyId == CompanyId

                              select new GetAllDealSectionDto
                              {
                                  Id = m.Id,
                                  DealId = m.DealId,
                                  ChooseQuantity = m.ChooseQuantity,
                                  CompanyId = m.CompanyId,
                                  CategoryId = m.CategoryId,
                                  CategoryName = _context.Category.FirstOrDefault(x => x.Id == m.CategoryId).Name,

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
