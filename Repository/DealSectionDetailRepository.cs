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
                var DealSectionDetailToCreate = new DealSectionDetail
                {
                    //DealId= dtoData.DealId,
                    DealSectionId = dtoData.DealSectionId,
                    ItemId = dtoData.ItemId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.DealSectionDetail.AddAsync(DealSectionDetailToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = DealSectionDetailToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditDealSectionDetail(int id, EditDealSectionDetailDto dtoData)
        {
            var objdealsectiondetail = await _context.DealSectionDetail.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objdealsectiondetail != null)
            {
                //objdealsectiondetail.DealId = dtoData.DealId;
                objdealsectiondetail.DealSectionId = dtoData.DealSectionId;
                objdealsectiondetail.ItemId = dtoData.ItemId;
                objdealsectiondetail.UpdateById = _LoggedIn_UserID;
                objdealsectiondetail.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

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

        //public async Task<ServiceResponse<object>> GetAllDealSectionDetail()
        //{
        //    var list = await (from m in _context.DealSectionDetail
        //                      where Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value) 

        //                      select new GetAllDealSectionDetailDto
        //                      {
        //                          Id = m.Id,
        //                          //DealId = m.DealId,
        //                          DealSectionId = m.DealSectionId,
        //                          ItemId= m.ItemId,
        //                          ItemName = _context.Items.FirstOrDefault(x => x.Id == m.ItemId).Name,
        //                          CreatedById = m.CretedById,
        //                          DateCreated = m.DateCreated,
        //                          UpdatedById = m.UpdateById,
        //                          DateModified= m.DateModified,

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
