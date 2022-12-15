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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PizzaOrder.Repository
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public OrderRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddOrder(AddOrderDto dtoData)
        {
            var objOrder = await (from u in _context.Orders
                                  where u.CompanyId != dtoData.CompanyId

                                  select new
                                  {
                                      OrderStatus = u.OrderStatus,
                                      CompanyId = u.CompanyId,
                                  }).FirstOrDefaultAsync();

            if (objOrder != null)
            {
                if (objOrder.CompanyId > 0 && dtoData.CompanyId != objOrder.CompanyId)
                {
                    _serviceResponse.Message = "This Company Id not exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = dtoData.CompanyId;
            }
            else
            {
                var OrderToCreate = new Order
                {
                    OrderStatus = dtoData.OrderStatus,
                    CompanyId = dtoData.CompanyId,
                };

                await _context.Orders.AddAsync(OrderToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = OrderToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditOrder(int id, EditOrderDto dtoData)
        {
            var objorder = await _context.Orders.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objorder != null)
            {
                objorder.OrderStatus = dtoData.OrderStatus;
                objorder.CompanyId = dtoData.CompanyId;

                _context.Orders.Update(objorder);
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

        public async Task<ServiceResponse<object>> GetAllOrder()
        {
            var list = await (from m in _context.Orders

                              where m.CompanyId == _LoggedIn_CompanyId && m.UserId == _LoggedIn_UserID
                                    && m.OrderStatus != (int)Helpers.Enums.OrderStatus.PreOrder

                              select new GetAllOrdersDto
                              {

                                  Id = m.Id,
                                  UserId = _LoggedIn_UserID,
                                  OrderStatus = ((Helpers.Enums.OrderStatus)m.OrderStatus).ToString(),
                                  PaymentMethodType = m.PaymentMethodType,
                                  TotalAmount = m.TotalAmount,
                                  DeliveryAddress = m.DeliveryAddress,
                                  DeliveryCharges = m.DeliveryCharges,
                                  CompanyId = m.CompanyId,
                                  DateCreated = m.DateCreated,

                                  ObjGetAllOrderDetail = (from n in _context.OrderDetail
                                                          where n.OrderId == m.Id

                                                          select new GetAllOrderDetailDto
                                                          {
                                                              Id = n.Id,
                                                              UserId = _LoggedIn_UserID,
                                                              OrderId = n.OrderId,
                                                              DealId = n.DealId,
                                                              DealName = _context.Deals.Where(x => x.Id == n.DealId).Select(x => x.Title).FirstOrDefault(),
                                                              DealPrice = _context.Deals.Where(x => x.Id == n.DealId).Select(x => x.Price).FirstOrDefault(),
                                                              CategoryId = n.CategoryId,
                                                              CategoryName = _context.Category.Where(x => x.Id == n.CategoryId).Select(x => x.Name).FirstOrDefault(),
                                                              ItemId = n.ItemId,
                                                              ItemName = _context.Items.Where(x => x.Id == n.ItemId).Select(x => x.Name).FirstOrDefault(),
                                                              ItemSizeId = n.ItemSizeId,
                                                              ItemSizeName = _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.SizeDescription).FirstOrDefault(),
                                                              ItemSizePrice = _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.Price).FirstOrDefault(),
                                                              CrustId = n.CrustId,
                                                              CrustName = _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Name).FirstOrDefault(),
                                                              CrustPrice = _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Price).FirstOrDefault(),

                                                              ObjGetAllTopping = (from p in _context.Toppings
                                                                                  where p.OrderDetailId == n.Id

                                                                                  select new GetAllToppingDto
                                                                                  {
                                                                                      Id = p.Id,
                                                                                      Name = p.Name,
                                                                                      Price = p.Price,
                                                                                      ItemId = p.ItemId,
                                                                                      ItemSizeId = p.ItemSizeId,
                                                                                      CategoryId = p.CategoryId,
                                                                                      CompanyId = p.CompanyId,

                                                                                  }).ToList(),

                                                              Quantity = n.Quantity,
                                                              CompanyId = n.CompanyId,
                                                              OrderType = ((Helpers.Enums.OrderType)n.OrderType).ToString(),
                                                              Instructions = n.Instructions,
                                                              SubTotal = n.SubTotal,
                                                          }).ToList(),
                              }).ToListAsync();

            //                  select new GetAllOrdersDto
            //                  {
            //                      Id = m.Id,
            //                      OrderStatus = ((Helpers.Enums.OrderStatus)m.OrderStatus).ToString(),
            //                      UserId = m.UserId,
            //                      CompanyId = m.CompanyId,
            //                      DealId = n.DealId,
            //                      DealName = _context.Deals.Where(x => x.Id == n.DealId).Select(x => x.Title).FirstOrDefault(),
            //                      DealPrice = _context.Deals.Where(x => x.Id == n.DealId).Select(x => x.Price).FirstOrDefault(),
            //                      CategoryId = n.CategoryId,
            //                      CategoryName = _context.Category.Where(x => x.Id == n.CategoryId).Select(x => x.Name).FirstOrDefault(),
            //                      ItemId = n.ItemId,
            //                      ItemName = _context.Items.Where(x => x.Id == n.ItemId).Select(x => x.Name).FirstOrDefault(),
            //                      ItemSizeId = n.ItemSizeId,
            //                      ItemSizeName = _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.SizeDescription).FirstOrDefault(),
            //                      ItemSizePrice = _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.Price).FirstOrDefault(),
            //                      CrustId = n.CrustId,
            //                      CrustName = _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Name).FirstOrDefault(),
            //                      CrustPrice = _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Price).FirstOrDefault(),

                              //                      ObjGetAllTopping = (from p in _context.Toppings
                              //                                          where p.OrderDetailId == n.Id

                              //                                          select new GetAllToppingDto
                              //                                          {
                              //                                              Id = p.Id,
                              //                                              Name = p.Name,
                              //                                              Price = p.Price,
                              //                                              ItemId = p.ItemId,
                              //                                              ItemSizeId = p.ItemSizeId,
                              //                                              CategoryId = p.CategoryId,
                              //                                              CompanyId = p.CompanyId,

                              //                                          }).ToList(),

                              //                      Quantity = n.Quantity,
                              //                      OrderType = ((Helpers.Enums.OrderType)n.OrderType).ToString(),
                              //                      Instructions = n.Instructions,
                              //                      SubTotal = n.SubTotal,
                              //                      DeliveryCharges = n.DeliveryCharges,
                              //                      // TotalAmount = (n.ItemSizeId != null? _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.Price).FirstOrDefault():0 +
                              //                      //  n.CrustId != null? _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Price).FirstOrDefault() : 0 ),

                              //                      //TotalTopingAmount= _context.Toppings.Where(x => x.OrderDetailId == n.Id).Sum(m => m.Price) > 0 ? _context.Toppings.Where(x => x.OrderDetailId == n.Id).Sum(m => m.Price) : 0 ,

                              //                  }).ToListAsync();



                              //var list = await (from m in _context.Orders
                              //                  where m.UserId == _LoggedIn_UserID && m.CompanyId == CompanyId

                              //                  select new GetAllOrderDto
                              //                  {
                              //                      Id = m.Id,
                              //                      OrderStatus = ((Helpers.Enums.OrderStatus)m.OrderStatus).ToString(),
                              //                      UserId = m.UserId,
                              //                      CompanyId = m.CompanyId,

                              //                      ObjGetAllOrderDetail = (from n in _context.OrderDetail where n.OrderId == m.Id

                              //                                              select new GetAllOrderDetailDto
                              //                                              {
                              //                                                  Id = n.Id,
                              //                                                  UserId = _LoggedIn_UserID,
                              //                                                  OrderId = n.OrderId,
                              //                                                  DealId = n.DealId,
                              //                                                  DealName = _context.Deals.Where(x => x.Id == n.DealId).Select(x => x.Title).FirstOrDefault(),
                              //                                                  DealPrice = _context.Deals.Where(x => x.Id == n.DealId).Select(x => x.Price).FirstOrDefault(),
                              //                                                  CategoryId = n.CategoryId,
                              //                                                  CategoryName = _context.Category.Where(x => x.Id == n.CategoryId).Select(x => x.Name).FirstOrDefault(),
                              //                                                  ItemId = n.ItemId,
                              //                                                  ItemName = _context.Items.Where(x => x.Id == n.ItemId).Select(x => x.Name).FirstOrDefault(),
                              //                                                  ItemSizeId = n.ItemSizeId,
                              //                                                  ItemSizeName = _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.SizeDescription).FirstOrDefault(),
                              //                                                  ItemSizePrice = _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.Price).FirstOrDefault(),
                              //                                                  CrustId = n.CrustId,
                              //                                                  CrustName = _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Name).FirstOrDefault(),
                              //                                                  CrustPrice = _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Price).FirstOrDefault(),
                              //                                                  ToppingId = n.ToppingId,
                              //                                                  ToppingName = _context.Toppings.Where(x => x.Id == n.ToppingId).Select(x => x.Name).FirstOrDefault(),
                              //                                                  ToppingPrice = _context.Toppings.Where(x => x.Id == n.ToppingId).Select(x => x.Price).FirstOrDefault(),
                              //                                                  Quantity = n.Quantity,
                              //                                                  CompanyId = m.CompanyId,
                              //                                                  OrderType = ((Helpers.Enums.OrderType)n.OrderType).ToString(),
                              //                                                  Instructions = n.Instructions,

                              //                                              }).ToList(),

                              //                  }).ToListAsync();



                              //var TotalBillAmount = list.Sum(m => Convert.ToDecimal(m.TotalAmount, CultureInfo.InvariantCulture));
                              //var totaltopingAmount = list.Sum(m => Convert.ToDecimal(m.TotalTopingAmount, CultureInfo.InvariantCulture));
                              //var TotalBillAmountDisplay = TotalBillAmount + totaltopingAmount;


                              //var TotalBillAmountDisplay = list.Sum(m => Convert.ToDecimal(m.SubTotal, CultureInfo.InvariantCulture));

                              //var objTotalAmount = new Order
                              //{
                              //    TotalAmount = Convert.ToInt32(TotalBillAmountDisplay)
                              //};
                              //_context.Orders.Update(objTotalAmount);
                              //await _context.SaveChangesAsync();


                              //var ObjDeliveryCharges = list[0].DeliveryCharges;


            if (list.Count > 0)
            {
                _serviceResponse.Data = new { list };
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

        public async Task<ServiceResponse<object>> GetPaymentType(int id)
        {
            if (id != 0)
            {
                var objPaymentType = new GetPaymentTypeDto
                {
                    PaymentType = ((Helpers.Enums.PaymentType)id).ToString(),
                };

                _serviceResponse.Data = objPaymentType;
                _serviceResponse.Success = true;
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
            }

            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> AddCheckOutDetails(AddCheckOutDetailsDto dtoData)
        {
            var objCheckOutDetail = await _context.Orders.FirstOrDefaultAsync(s => s.Id.Equals(dtoData.OrderId));

            if (objCheckOutDetail != null)
            {
                objCheckOutDetail.PaymentMethodType = ((Helpers.Enums.PaymentType)dtoData.PaymentMethodType).ToString();
                objCheckOutDetail.DeliveryAddress = dtoData.DeliveryAddress;
                objCheckOutDetail.OrderStatus = (int)Helpers.Enums.OrderStatus.Pending;

                _context.Orders.Update(objCheckOutDetail);
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


        public async Task<ServiceResponse<object>> GetCartList()
        {
            var objcartlist = await _context.Orders.FirstOrDefaultAsync(x => x.UserId.Equals(_LoggedIn_UserID) && x.OrderStatus == (int)Helpers.Enums.OrderStatus.PreOrder);
            if (objcartlist != null)
            {
                var data = new GetOrderByIdDto
                {
                    Id = objcartlist.Id,
                    UserId = _LoggedIn_UserID,
                    UserName = _LoggedIn_UserName,
                    ContactNumber = _context.Users.Where(x => x.Id == objcartlist.UserId).Select(x=> x.ContactNumber).FirstOrDefault(),
                    OrderStatus = ((Helpers.Enums.OrderStatus)objcartlist.OrderStatus).ToString(),
                    PaymentMethodType = objcartlist.PaymentMethodType,
                    DeliveryAddress = _context.Users.Where(x => x.Id == objcartlist.UserId).Select(x => x.Address).FirstOrDefault(),
                    CompanyId = objcartlist.CompanyId,

                    ObjGetAllOrderDetail = (from n in _context.OrderDetail
                                            where n.OrderId == objcartlist.Id

                                            select new GetAllOrderDetailDto
                                            {
                                                Id = n.Id,
                                                UserId = _LoggedIn_UserID,
                                                OrderId = n.OrderId,
                                                DealId = n.DealId,
                                                DealName = _context.Deals.Where(x => x.Id == n.DealId).Select(x => x.Title).FirstOrDefault(),
                                                DealPrice = _context.Deals.Where(x => x.Id == n.DealId).Select(x => x.Price).FirstOrDefault(),
                                                CategoryId = n.CategoryId,
                                                CategoryName = _context.Category.Where(x => x.Id == n.CategoryId).Select(x => x.Name).FirstOrDefault(),
                                                ItemId = n.ItemId,
                                                ItemName = _context.Items.Where(x => x.Id == n.ItemId).Select(x => x.Name).FirstOrDefault(),
                                                ItemSizeId = n.ItemSizeId,
                                                ItemSizeName = _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.SizeDescription).FirstOrDefault(),
                                                ItemSizePrice = _context.ItemSize.Where(x => x.Id == n.ItemSizeId).Select(x => x.Price).FirstOrDefault(),
                                                CrustId = n.CrustId,
                                                CrustName = _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Name).FirstOrDefault(),
                                                CrustPrice = _context.Crusts.Where(x => x.Id == n.CrustId).Select(x => x.Price).FirstOrDefault(),

                                                ObjGetAllTopping = (from p in _context.Toppings
                                                                    where p.OrderDetailId == n.Id

                                                                    select new GetAllToppingDto
                                                                    {
                                                                        Id = p.Id,
                                                                        Name = p.Name,
                                                                        Price = p.Price,
                                                                        ItemId = p.ItemId,
                                                                        ItemSizeId = p.ItemSizeId,
                                                                        CategoryId = p.CategoryId,
                                                                        CompanyId = p.CompanyId,

                                                                    }).ToList(),

                                                Quantity = n.Quantity,
                                                CompanyId = n.CompanyId,
                                                OrderType = ((Helpers.Enums.OrderType)n.OrderType).ToString(),
                                                Instructions = n.Instructions,
                                                SubTotal = n.SubTotal,
                                            }).ToList(),

                   //TotalAmount = ObjGetAllOrderDetail.Sum(n => Convert.ToDecimal(n.SubTotal, CultureInfo.InvariantCulture)),
                };
                data.TotalAmount = data.ObjGetAllOrderDetail.Sum(n => Convert.ToInt32(n.SubTotal, CultureInfo.InvariantCulture));

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
