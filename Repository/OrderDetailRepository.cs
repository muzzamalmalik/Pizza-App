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
            var objOrderDetail = await (from u in _context.OrderDetail
                                        where u.CompanyId != dtoData.CompanyId

                                        select new
                                        {
                                            CompanyId = u.CompanyId,
                                        }).FirstOrDefaultAsync();

            if (objOrderDetail != null)
            {
                if (objOrderDetail.CompanyId > 0 && objOrderDetail.CompanyId != dtoData.CompanyId)
                {
                    _serviceResponse.Message = "This Company Id not exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = dtoData.CompanyId;
            }
            else
            {
                var OrderDetailToCreate = new OrderDetail
                {
                    OrderId= dtoData.OrderId,
                    DealId = dtoData.DealId,
                    CategoryId= dtoData.CategoryId,
                    ItemId= dtoData.ItemId,
                    ItemSizeId= dtoData.ItemSizeId,
                    CrustId = dtoData.CrustId,
                    ToppingId= dtoData.ToppingId,
                    CompanyId = dtoData.CompanyId,
                    Quantity= dtoData.Quantity,
                    OrderType= dtoData.OrderType,
                    Instructions = dtoData.Instructions,
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
            //var objAddToCart = await (from u in _context.OrderDetail
            //                            where u.CompanyId != dtoData.CompanyId

            //                            select new
            //                            {
            //                                CompanyId = u.CompanyId,
            //                            }).FirstOrDefaultAsync();

            //if (objAddToCart != null)
            //{
            //    if (objAddToCart.CompanyId > 0 && objAddToCart.CompanyId != dtoData.CompanyId)
            //    {
            //        _serviceResponse.Message = "This Company Id not exists";
            //    }
            //    _serviceResponse.Success = false;
            //    _serviceResponse.Data = dtoData.CompanyId;
            //}
            //else
            //{


            var objpreorder = await _context.Orders.FirstOrDefaultAsync(x => x.UserId.Equals(_LoggedIn_UserID) && x.OrderStatus == (int)Helpers.Enums.OrderStatus.PreOrder);

            if(objpreorder == null)
            {
                var OrderToCreate = new Order
                {
                    OrderStatus = 1,
                    UserId = _LoggedIn_UserID,
                    CompanyId = _LoggedIn_CompanyId,
                };

                await _context.Orders.AddAsync(OrderToCreate);
                await _context.SaveChangesAsync();

                var AddToCartToCreate = new OrderDetail
                {
                    OrderId = OrderToCreate.Id,
                    DealId = dtoData.DealId,
                    CategoryId = dtoData.CategoryId,
                    ItemId = dtoData.ItemId,
                    ItemSizeId = dtoData.ItemSizeId,
                    CrustId = dtoData.CrustId,
                    CompanyId = _LoggedIn_CompanyId,
                    Quantity = dtoData.Quantity,
                    OrderType = dtoData.OrderType,
                    Instructions = dtoData.Instructions,
                    SubTotal = dtoData.SubTotal,
                };

                await _context.OrderDetail.AddAsync(AddToCartToCreate);
                await _context.SaveChangesAsync();



                if (dtoData.objtopping != null && dtoData.objtopping.Count > 0)
                {
                    for (int a = 0; a < dtoData.objtopping.Count; a++)
                    {
                        var item1 = dtoData.objtopping[a];
                        var objtopings = new Topping
                        {
                            OrderDetailId = AddToCartToCreate.Id,
                            CategoryId = item1.CategoryId,
                            CompanyId = _LoggedIn_CompanyId,
                            ItemId = item1.ItemId,
                            Name = item1.Name,
                            Price = item1.Price,
                            ItemSizeId = item1.ItemSizeId,

                        };
                        await _context.Toppings.AddAsync(objtopings);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                var AddToCartToCreate1 = new OrderDetail
                {
                    OrderId = objpreorder.Id,
                    DealId = dtoData.DealId,
                    CategoryId = dtoData.CategoryId,
                    ItemId = dtoData.ItemId,
                    ItemSizeId = dtoData.ItemSizeId,
                    CrustId = dtoData.CrustId,
                    CompanyId = _LoggedIn_CompanyId,
                    Quantity = dtoData.Quantity,
                    OrderType = dtoData.OrderType,
                    Instructions = dtoData.Instructions,
                    SubTotal = dtoData.SubTotal,
                };

                await _context.OrderDetail.AddAsync(AddToCartToCreate1);
                await _context.SaveChangesAsync();



                if (dtoData.objtopping != null && dtoData.objtopping.Count > 0)
                {
                    for (int a = 0; a < dtoData.objtopping.Count; a++)
                    {
                        var item1 = dtoData.objtopping[a];
                        var objtopings = new Topping
                        {
                            OrderDetailId = AddToCartToCreate1.Id,
                            CategoryId = item1.CategoryId,
                            CompanyId = _LoggedIn_CompanyId,
                            ItemId = item1.ItemId,
                            Name = item1.Name,
                            Price = item1.Price,
                            ItemSizeId = item1.ItemSizeId,

                        };
                        await _context.Toppings.AddAsync(objtopings);
                        await _context.SaveChangesAsync();
                    }
                }
            }


                //_serviceResponse.Data = OrderDetailToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditOrderDetail(int id, EditOrderDetailDto dtoData)
        {
            var objorderdetail = await _context.OrderDetail.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objorderdetail != null)
            {
                objorderdetail.OrderId = dtoData.OrderId;
                objorderdetail.DealId = dtoData.DealId;
                objorderdetail.CategoryId = dtoData.CategoryId;
                objorderdetail.ItemId = dtoData.ItemId;
                objorderdetail.ItemSizeId= dtoData.ItemSizeId;
                objorderdetail.CrustId= dtoData.CrustId;
                objorderdetail.ToppingId = dtoData.ToppingId;
                objorderdetail.CompanyId = dtoData.CompanyId;
                objorderdetail.Quantity= dtoData.Quantity;
                objorderdetail.OrderType= dtoData.OrderType;
                objorderdetail.Instructions= dtoData.Instructions;
                objorderdetail.SubTotal = dtoData.SubTotal;

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
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllOrderDetail(int CompanyId)
        {
            var list = await (from m in _context.OrderDetail
                              where m.CompanyId == CompanyId 

                              select new GetAllOrderDetailDto
                              {
                                  Id = m.Id,
                                  UserId = _LoggedIn_UserID,
                                  OrderId= m.OrderId,
                                  DealId = m.DealId,
                                  DealName = _context.Deals.Where(x => x.Id == m.DealId).Select(x => x.Title).FirstOrDefault(),
                                  DealPrice = _context.Deals.Where(x => x.Id == m.DealId).Select(x => x.Price).FirstOrDefault(),
                                  CategoryId = m.CategoryId,
                                  CategoryName = _context.Category.Where(x => x.Id == m.CategoryId).Select(x => x.Name).FirstOrDefault(),
                                  ItemId = m.ItemId,
                                  ItemName = _context.Items.Where(x => x.Id == m.ItemId).Select(x => x.Name).FirstOrDefault(),
                                  ItemSizeId = m.ItemSizeId,
                                  ItemSizeName = _context.ItemSize.Where(x => x.Id == m.ItemSizeId).Select(x => x.SizeDescription).FirstOrDefault(),
                                  ItemSizePrice = _context.ItemSize.Where(x => x.Id == m.ItemSizeId).Select(x => x.Price).FirstOrDefault(),
                                  CrustId = m.CrustId,
                                  CrustName = _context.Crusts.Where(x => x.Id == m.CrustId).Select(x => x.Name).FirstOrDefault(),
                                  CrustPrice = _context.Crusts.Where(x => x.Id == m.CrustId).Select(x => x.Price).FirstOrDefault(),

                                  ObjGetAllTopping = (from p in _context.Toppings where p.OrderDetailId == m.Id

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

                                  Quantity = m.Quantity,
                                  CompanyId = m.CompanyId,
                                  OrderType= ((Helpers.Enums.OrderType)m.OrderType).ToString(),
                                  Instructions = m.Instructions,
                                  SubTotal = m.SubTotal,

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
                    OrderId= objorderdetail.OrderId,
                    DealId= objorderdetail.DealId,
                    CategoryId= objorderdetail.CategoryId,
                    ItemId= objorderdetail.ItemId,
                    ItemSizeId= objorderdetail.ItemSizeId,
                    CrustId= objorderdetail.CrustId,
                    ToppingId= objorderdetail.ToppingId,
                    Quantity = objorderdetail.Quantity,
                    CompanyId = objorderdetail.CompanyId,
                    OrderType= ((Helpers.Enums.OrderType)objorderdetail.OrderType).ToString(),
                    Instructions = objorderdetail.Instructions,
                    SubTotal= objorderdetail.SubTotal,
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

        public async Task<ServiceResponse<object>> DeleteOrderDetailById(int id)
        {
            var objorderdetail = await _context.OrderDetail.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (objorderdetail == null)
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            else
            {
                _context.OrderDetail.Remove(objorderdetail);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Deleted;
            }

            return _serviceResponse;
        }
    }
}
