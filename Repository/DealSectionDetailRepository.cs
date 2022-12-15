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
    public class DealSectionDetailRepository : BaseRepository, IDealSectionDetailRepository
    {

        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public DealSectionDetailRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddDealSectionDetail(AddDealSectionDetailDto dtoData)
        {
            var objDealSectionDetail = await (from u in _context.DealSectionDetail
                                        where u.CompanyId != dtoData.CompanyId

                                        select new
                                        {
                                            DealId = u.DealId,
                                            DealSectionId = u.DealSectionId,
                                            ItemId = u.ItemId,
                                            CompanyId = u.CompanyId,
                                        }).FirstOrDefaultAsync();

            if (objDealSectionDetail != null)
            {
                if (objDealSectionDetail.CompanyId > 0 && dtoData.CompanyId != objDealSectionDetail.CompanyId)
                {
                    _serviceResponse.Message = "This Company Id not exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = dtoData.CompanyId;
            }
            else
            {
                var DealSectionDetailToCreate = new DealSectionDetail
                {
                    DealId= dtoData.DealId,
                    DealSectionId = dtoData.DealSectionId,
                    ItemId = dtoData.ItemId,
                    CompanyId = dtoData.CompanyId,
                };

                await _context.DealSectionDetail.AddAsync(DealSectionDetailToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = DealSectionDetailToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditDealSectionDetail(int id, EditDealSectionDetailDto dtoData)
        {
            var objdealsectiondetail = await _context.DealSectionDetail.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objdealsectiondetail != null)
            {
                objdealsectiondetail.DealId = dtoData.DealId;
                objdealsectiondetail.DealSectionId = dtoData.DealSectionId;
                objdealsectiondetail.ItemId = dtoData.ItemId;
                objdealsectiondetail.CompanyId = dtoData.CompanyId;

                _context.DealSectionDetail.Update(objdealsectiondetail);
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

        public async Task<ServiceResponse<object>> GetAllDealSectionDetail(int CompanyId)
        {
            var list = await (from m in _context.DealSectionDetail
                              where m.CompanyId == CompanyId

                              select new GetAllDealSectionDetailDto
                              {
                                  Id = m.Id,
                                  DealId = m.DealId,
                                  DealSectionId = m.DealSectionId,
                                  ItemId= m.ItemId,
                                  ItemName = _context.Items.FirstOrDefault(x => x.Id == m.ItemId).Name,
                                  CompanyId = m.CompanyId,

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
