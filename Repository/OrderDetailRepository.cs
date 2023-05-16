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
    public class OrderDetailRepository : BaseRepository, IOrderDetailRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public OrderDetailRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }
        public async Task<ServiceResponse<object>> AddOrderDetail(AddOrderDetailDto dtoData)
        {
            if (dtoData != null)
            {
                var OrderDetailToCreate = new OrderDetail
                {
                    OrderId = dtoData.OrderId,
                    DealId = dtoData.DealId,
                    //CategoryId = dtoData.CategoryId,
                    ItemId = dtoData.ItemId,
                    ItemSizeId = dtoData.ItemSizeId,
                    CrustId = dtoData.CrustId,
                    //ToppingId = dtoData.ToppingId,
                    //CompanyId = dtoData.CompanyId,
                    Quantity = dtoData.Quantity,
                    //OrderType = dtoData.OrderType,
                    //Instructions = dtoData.Instructions,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.OrderDetail.AddAsync(OrderDetailToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = OrderDetailToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }

            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> AddToCartCall(AddToCartCallDto dtoData)        
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(x => x.UserId.Equals(_LoggedIn_UserID) && x.CompanyId == dtoData.CompanyId && x.OrderStatus == (int)Helpers.Enums.OrderStatus.PreOrder);
            if (existingOrder == null)
            {
                var OrderToCreate = new Order
                {
                    OrderStatus = (int)Helpers.Enums.OrderStatus.PreOrder,
                    UserId = _LoggedIn_UserID,
                    CompanyId = dtoData.CompanyId,
                    CretedById = _LoggedIn_UserID,
                    OrderType=dtoData.OrderType,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.Orders.AddAsync(OrderToCreate);
                await _context.SaveChangesAsync();

                dtoData.OrderId = OrderToCreate.Id;
            }
            else if (existingOrder.CompanyId == dtoData.CompanyId)
            {
                dtoData.OrderId = existingOrder.Id;
            }
            else
            {
                var OrderToCreate = new Order
                {
                    OrderStatus = (int)Helpers.Enums.OrderStatus.PreOrder,
                    UserId = _LoggedIn_UserID,
                    CompanyId = dtoData.CompanyId,
                    CretedById = _LoggedIn_UserID,
                    OrderType= dtoData.OrderType,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.Orders.AddAsync(OrderToCreate);
                await _context.SaveChangesAsync();

                dtoData.OrderId = OrderToCreate.Id;
            }
            if (dtoData.objOrderDetail != null)
            {

                foreach (var item in dtoData.objOrderDetail)
                {

                    if (item.DealId == 0 || item.DealId == null)
                    {
                        var objOrderDetail = new OrderDetail
                        {
                            OrderId = dtoData.OrderId,
                            DealId = item.DealId,
                            ItemId = item.ItemId,
                            ItemSizeId = item.ItemSizeId,
                            CrustId = item.CrustId,
                            Quantity = item.Quantity,
                            //OrderType = item.OrderType,
                            //SubTotal = item.SubTotal,
                            CretedById = _LoggedIn_UserID,
                            DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),

                        };
                        await _context.OrderDetail.AddAsync(objOrderDetail);
                        await _context.SaveChangesAsync();

                        foreach (var itemAdditional in item.objAdditionalDetails)
                        {
                            var objAdditionalDetails = new OrderDetailAdditionalDetails
                            {
                                Id = itemAdditional.Id,
                                ReferenceId = itemAdditional.ReferenceId,
                                OrderDetailId = objOrderDetail.Id,
                                ReferenceTypeId = itemAdditional.ReferenceTypeId,
                            };

                            await _context.OrderDetailAdditionalDetails.AddAsync(objAdditionalDetails);
                            await _context.SaveChangesAsync();

                        }
                    }
                    else
                    {
                        int i = 0;

                        foreach (var dealItem in item.objDeals)
                        {
                            var objOrderDetail = new OrderDetail
                            {
                                OrderId = dtoData.OrderId,
                                DealId = dealItem.DealId,
                                ItemId = dealItem.ItemId,
                                //ItemSizeId = item.ItemSizeId,
                                //CrustId = item.CrustId,
                                Quantity = item.Quantity,
                               // OrderType = dealItem.OrderType,
                                //SubTotal = item.SubTotal,
                                BillGroup = _context.OrderDetail.Where(x => x.OrderId == dtoData.OrderId).Count() > 0 ?
                                _context.OrderDetail.Where(x => x.OrderId == dtoData.OrderId && x.DealId == dealItem.DealId).Count() > 0 && i == 0 ?
                                 _context.OrderDetail.Where(x => x.OrderId == dtoData.OrderId && x.DealId == dealItem.DealId).Max(x => x.BillGroup) + 1 : i == 0 ? 1 : i : 1,
                                CretedById = _LoggedIn_UserID,
                                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),

                            };
                            i = objOrderDetail.BillGroup;
                            await _context.OrderDetail.AddAsync(objOrderDetail);
                            await _context.SaveChangesAsync();
                        }

                    }

                }
            }

            //_serviceResponse.Data = OrderDetailToCreate;
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;

            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> EditOrderDetail(int id, OrderDetailDto dtoData)
        {
            if(id>0)
            {
                var objorderdetail = await _context.OrderDetail.FirstOrDefaultAsync(s => s.Id.Equals(id));
                if (objorderdetail != null)
                {

                    //objorderdetail.OrderId = dtoData.OrderId;
                    objorderdetail.DealId = dtoData.IsDeal == true?dtoData.DealId:null;
                    objorderdetail.ItemId = dtoData.ItemId;
                    objorderdetail.ItemSizeId = dtoData.IsDeal == true ?null: dtoData.ItemSizeId==0?null: dtoData.ItemSizeId;
                    objorderdetail.CrustId = dtoData.IsDeal == true ? null : dtoData.CrustId==0?null: dtoData.CrustId;
                    objorderdetail.Quantity = dtoData.Quantity;
                    objorderdetail.UpdateById = _LoggedIn_UserID;
                    objorderdetail.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                    _context.OrderDetail.Update(objorderdetail);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Updated;
                                        
                }
                else
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                }
            }
            else
            {
                if(dtoData.IsDeal==true)
                {
                    if(dtoData.objDeals!=null)
                    {
                        int i = 0;

                        foreach (var dealItem in dtoData.objDeals)
                        {
                            var objOrderDetail = new OrderDetail
                            {
                                OrderId = dtoData.OrderId,
                                DealId = dealItem.DealId,
                                ItemId = dealItem.ItemId,
                                Quantity = dtoData.Quantity,
                                BillGroup = _context.OrderDetail.Where(x => x.OrderId == dtoData.OrderId).Count() > 0 ?
                                _context.OrderDetail.Where(x => x.OrderId == dtoData.OrderId && x.DealId == dealItem.DealId).Count() > 0 && i == 0 ?
                                 _context.OrderDetail.Where(x => x.OrderId == dtoData.OrderId && x.DealId == dealItem.DealId).Max(x => x.BillGroup) + 1 : i == 0 ? 1 : i : 1,
                                CretedById = _LoggedIn_UserID,
                                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),

                            };
                            i = objOrderDetail.BillGroup;
                            await _context.OrderDetail.AddAsync(objOrderDetail);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    var objOrderDetail = new OrderDetail
                    {
                        OrderId = dtoData.OrderId,
                        DealId = dtoData.DealId,
                        ItemId = dtoData.ItemId,
                        ItemSizeId = dtoData.ItemSizeId,
                        CrustId = dtoData.CrustId,
                        Quantity = dtoData.Quantity,
                        CretedById = _LoggedIn_UserID,
                        DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),                     
                        BillGroup = 0,                        

                    };
                    await _context.OrderDetail.AddAsync(objOrderDetail);
                    await _context.SaveChangesAsync();
                }
            }
           
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetAllOrderDetail()
        {
            var list = await (from m in _context.OrderDetail
                              where _LoggedIn_UserTypeId==1?true:m.CretedById==_LoggedIn_UserID
                              select new GetAllOrderDetailDto
                              {
                                  Id = m.Id,
                                  //UserId = _LoggedIn_UserID,
                                  OrderId = m.OrderId,
                                  DealId = m.DealId,
                                  DealName = m.ObjDeal.Title,
                                  DealPrice = m.ObjDeal.Price,
                                  //CategoryId = m.CategoryId,
                                  //CategoryName = m.ObjCategory.Name,
                                  ItemId = m.ItemId,
                                  ItemName = m.ObjItem.Name,
                                  ItemSizeId = m.ItemSizeId,
                                  ItemSizeName = m.ObjItemSize.SizeDescription,
                                  ItemSizePrice = m.ObjItemSize.Price>0? m.ObjItemSize.Price:m.ObjItem.Price,
                                  CrustId = m.CrustId,
                                  CrustName = m.ObjCrust.Name,
                                  CrustPrice = m.ObjCrust.Price,

                                  ObjAdditionalDetails = (from p in _context.OrderDetailAdditionalDetails
                                                          where p.OrderDetailId == m.Id

                                                          select new OrderDetailAdditionalDetailsDto
                                                          {
                                                              Id = p.Id,
                                                              ReferenceId = p.ReferenceId,
                                                              OrderDetailId = p.OrderDetailId,
                                                              ReferenceTypeId = p.ReferenceTypeId,

                                                          }).ToList(),

                                  Quantity = m.Quantity,
                                  //CompanyId = m.CompanyId,
                                 // OrderType = ((Helpers.Enums.OrderType)m.OrderType).ToString(),
                                  //Instructions = m.Instructions,
                                  //SubTotal = m.SubTotal,
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
        public async Task<ServiceResponse<object>> GetOrderDetailById(int id)
        {
            var objorderdetail = await _context.OrderDetail.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (objorderdetail != null)
            {
                var data = new GetOrderDetailByIdDto
                {
                    Id = objorderdetail.Id,
                    UserId = _LoggedIn_UserID,
                    OrderId = objorderdetail.OrderId,
                    DealId = objorderdetail.DealId,
                    //CategoryId= objorderdetail.CategoryId,
                    ItemId = objorderdetail.ItemId,
                    ItemSizeId = objorderdetail.ItemSizeId,
                    CrustId = objorderdetail.CrustId,
                    //ToppingId= objorderdetail.ToppingId,
                    Quantity = objorderdetail.Quantity,
                    //CompanyId = objorderdetail.CompanyId,
                    //OrderType = ((Helpers.Enums.OrderType)objorderdetail.OrderType).ToString(),
                    //Instructions = objorderdetail.Instructions,
                    //SubTotal = objorderdetail.SubTotal,
                    CreatedById = objorderdetail.CretedById,
                    DateCreated = objorderdetail.DateCreated,
                    UpdatedById = objorderdetail.UpdateById,
                    DateModified = objorderdetail.DateModified,
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
        public async Task<ServiceResponse<object>> DeleteOrderDetailById(string id)
        {
            var orderDetailIdlist = id.Split(',').Select(Int32.Parse).ToList();
            foreach (var i in orderDetailIdlist)
            {
                var objorderdetail = await _context.OrderDetail.FirstOrDefaultAsync(x => x.Id.Equals(i));

                if (objorderdetail != null)
                {
                    var objOrderDetailAddotional = _context.OrderDetailAdditionalDetails.Where(x => x.OrderDetailId.Equals(objorderdetail.Id)).ToList();
                    foreach (var item in objOrderDetailAddotional)
                    {
                        _context.OrderDetailAdditionalDetails.Remove(item);
                        await _context.SaveChangesAsync();
                    }

                    _context.OrderDetail.Remove(objorderdetail);
                    await _context.SaveChangesAsync();

                    var count = _context.OrderDetail.Where(x => x.OrderId == objorderdetail.OrderId).Count();
                    if (count == 0)
                    {
                        var objOrder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == objorderdetail.OrderId);

                        _context.Orders.Remove(objOrder);
                        await _context.SaveChangesAsync();
                    }

                }
                else
                {
                    _serviceResponse.Data = null;
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                }
            }

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Deleted;

            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> DeleteOrderDetailByOrderDetailId(int id, int? dealId)
        {
            List<OrderDetail> list=null;
            var objorder = await _context.OrderDetail.FirstOrDefaultAsync(x => x.Id.Equals(id) && x.DealId.Equals(dealId));
            list =  await _context.OrderDetail.Where(x =>
                        dealId>0? (x.OrderId == objorder.OrderId&& x.DealId == dealId && x.BillGroup == objorder.BillGroup):(x.Id== id)).ToListAsync();

            if(list!=null)
            {
                _context.OrderDetail.RemoveRange(list);
                await _context.SaveChangesAsync();
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;

            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Deleted;

            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetDealItemsListById(int id,int CategoryId=0)
        {
            var list = await (from odtl in _context.OrderDetail
                                        where odtl.Id == id
                                        select new GetDealsItemsListDto
                                        {
                                            objItemList = (from it in _context.Items
                                                           where it.CategoryId== CategoryId
                                                           select new GetAllDealsItemDto
                                                           {
                                                               ItemId = it.Id,
                                                               ItemName = it.Name,
                                                               CategoryId= it.CategoryId
                                                           }).ToList(),
                                            Quantity = odtl.Quantity,
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
