using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Context;
using PizzaOrder.Helpers;
using System.Linq;
using PizzaOrder.IRepository;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PizzaOrder.Dtos;
using PizzaOrder.Models;
using System;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace PizzaOrder.Repository
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public ReportRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }
        public async Task<ServiceResponse<object>> GetOrderListForReport()
        {
            var list = await (from m in _context.Orders
                              join n in _context.OrderDetail on m.Id equals n.OrderId
                              join d in _context.Deals on n.DealId equals d.Id into deal
                              from ds in deal.DefaultIfEmpty()
                              join i in _context.Items on new { Id = n.ItemId, BllGroup = n.BillGroup } equals
                              new { Id = (int?)i.Id, BllGroup = 0 } into item
                              from it in item.DefaultIfEmpty()                                  
                              join a in _context.UserDeliveryAddress on m.CretedById equals a.UserId
                              group new { m, a, ds, it } by m.Id into k
                              orderby k.Max(q => q.m.Id) ascending
                              select new GetOrderListForReportDto
                              {
                                  Id = k.Max(q => q.m.Id),
                                  //Title = k.Max(q => q.ds.Title) + "," + k.Max(q => q.it.Name),
                                  Title = string.Join(",", k.Max(q => q.ds.Title), k.Max(q => q.it.Name)),
                                  PaymentMethodType = k.Max(q => q.m.PaymentMethodType),
                                  Status = ((Helpers.Enums.OrderStatus)k.Max(q => q.m.OrderStatus)).ToString(),
                                  //Rider=u.UserName,
                                  DeliveryAddress = k.Max(q => q.a.SecoundaryAddress),
                                  Date = k.Max(q => q.m.DateCreated),
                              }).ToListAsync();
            if (list.Count > 0)
            {
                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.RecordFound;
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetCustomerWiseOrderReport()
        {
            var list = await (from ord in _context.Orders
                              join odtl in _context.OrderDetail on ord.Id equals odtl.OrderId
                              join d in _context.Deals on odtl.DealId equals d.Id into deal
                              from del in deal.DefaultIfEmpty()
                              join i in _context.Items on new { Id = odtl.ItemId, BllGroup = odtl.BillGroup } equals
                              new { Id = (int?)i.Id, BllGroup = 0 } into item
                              from it in item.DefaultIfEmpty()
                              group new { ord,it,del, odtl } by new
                              {
                                  Id = ord.Id,
                                  Date = ord.DateCreated,
                                  ItemName = odtl.BillGroup == 0 ? it.Name : string.Empty,
                                  ItemId = odtl.BillGroup == 0 ? odtl.ItemId : 0,
                                  DealName = del.Title,
                                  Quantity = odtl.Quantity,
                                  BillGroup = odtl.DealId != null ? odtl.BillGroup : 0,
                                  Price=del.Price
                              } into grp
                              let its = (from itm in _context.ItemSize where itm.ItemId == grp.Key.ItemId select itm).FirstOrDefault()
                              select new GetOrderListForReportDto
                              {
                                  Id = grp.Key.Id,
                                  Date=grp.Key.Date,
                                  ItemName=grp.Key.ItemName,
                                  DealName= grp.Key.DealName,
                                  Price= grp.Key.BillGroup == 0 ?its.Price
                                        :grp.Key.Price,
                                  Quantity=grp.Key.Quantity
                              }).ToListAsync();
            if (list.Count > 0)
            {
                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.RecordFound;
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
