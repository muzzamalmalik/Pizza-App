using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Context;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using PizzaOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var OrderToCreate = new Order
            {
                OrderStatus = dtoData.OrderStatus,
                CompanyId = dtoData.CompanyId,
                CretedById = _LoggedIn_UserID,
                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
            };

            await _context.Orders.AddAsync(OrderToCreate);
            await _context.SaveChangesAsync();
            //_serviceResponse.Data = OrderToCreate;
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;


            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditOrder(int id, EditOrderDto dtoData)
        {
            var objorder = await _context.Orders.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objorder != null)
            {
                objorder.OrderStatus = dtoData.OrderStatus;
                objorder.CompanyId = dtoData.CompanyId;
                objorder.UpdateById = _LoggedIn_UserID;
                objorder.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

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


        public async Task<ServiceResponse<object>> GetOrderById(int id, int orderStatus, int page, int pageSize)
        {

            List<GetAllOrderDto> mainList = new List<GetAllOrderDto>();



            var listCount = _context.Orders.Where(x => id > 0 ? x.Id.Equals(id) : x.UserId == _LoggedIn_UserID &&
           orderStatus == (int)Helpers.Enums.OrderStatus.All ? x.OrderStatus != (int)Helpers.Enums.OrderStatus.PreOrder : x.OrderStatus == orderStatus).Count();

            var skip = pageSize * (page - 1);
            //var canPage = skip < listCount;

            var list = await _context.Orders.Where(x => id > 0 ? x.Id.Equals(id) : x.UserId == _LoggedIn_UserID &&
            orderStatus == (int)Helpers.Enums.OrderStatus.All ? x.OrderStatus != (int)Helpers.Enums.OrderStatus.PreOrder : x.OrderStatus == orderStatus).Skip(skip).Take(pageSize).ToListAsync();

            if (list != null)
            {
                foreach (var data in list)
                {
                    var objOrderdtls = (from odtl in _context.OrderDetail
                                        where odtl.OrderId == data.Id
                                        group odtl by new
                                        {
                                            Id = odtl.DealId == null ? odtl.Id : 0,
                                            odtl.OrderId,
                                            odtl.DealId,
                                            odtl.BillGroup
                                        } into gcs
                                        select new
                                        {
                                            id = gcs.Key.Id,
                                            orderId = gcs.Key.OrderId,
                                            dealId = gcs.Key.DealId,
                                            billGroup = gcs.Key.BillGroup
                                        }).ToList();

                    var record = new GetAllOrderDto
                    {
                        Id = data.Id,
                        UserId = data.UserId,
                        UserName = _context.Users.Where(x => x.Id == data.UserId).Select(x => x.FullName).FirstOrDefault(),
                        OrderStatus = ((Helpers.Enums.OrderStatus)data.OrderStatus).ToString(),
                        PaymentMethodType = data.PaymentMethodType,
                        DeliveryAddress = _context.Users.Where(x => x.Id == data.UserId).Select(x => x.Address).FirstOrDefault(),
                        EstimatedDeliveryTime = data.EstimatedDeliveryTime,
                        DeliveryCharges = data.DeliveryCharges,
                        //CompanyName = data.ObjCompany.Name,

                        ObjGetAllOrderDetail = (from n in objOrderdtls
                                                let odtlData = from idtl in _context.OrderDetail
                                                               where n.dealId == null && idtl.Id == n.id
                                                               select idtl

                                                select new GetAllOrderDetailDto
                                                {
                                                    Id = n.id,
                                                    OrderId = data.Id,
                                                    DealId = n.dealId,
                                                    BillGroup = n.billGroup,
                                                    objDeals = (from odldeal in _context.OrderDetail
                                                                where odldeal.OrderId == data.Id && (odldeal.DealId == (n.dealId == null ? 0 : n.dealId)) && odldeal.BillGroup == n.billGroup
                                                                select new DealDto
                                                                {
                                                                    OrderDetailId = odldeal.Id,
                                                                    dealId = odldeal.DealId,
                                                                    billGroup = odldeal.BillGroup,
                                                                    itemId = odldeal.ItemId,
                                                                    Name = odldeal.ObjItem.Name
                                                                }).ToList(),

                                                    DealName = _context.Deals.Where(x => x.Id == n.dealId).Select(x => x.Title).FirstOrDefault(),
                                                    DealPrice = _context.Deals.Where(x => x.Id == n.dealId).Select(x => x.Price).FirstOrDefault(),
                                                    ItemId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemId).FirstOrDefault(),
                                                    ItemName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ObjItem.Name).FirstOrDefault(),
                                                    ItemSizeId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? 0 : x.ItemSizeId).FirstOrDefault(),
                                                    ItemSizeName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? string.Empty : x.ObjItemSize.SizeDescription).FirstOrDefault(),
                                                    ItemSizePrice = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? 0 : x.ObjItemSize.Price).FirstOrDefault(),
                                                    CrustId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? 0 : x.CrustId).FirstOrDefault(),
                                                    CrustName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? string.Empty : x.ObjCrust.Name).FirstOrDefault(),
                                                    CrustPrice = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? 0 : x.ObjCrust.Price).FirstOrDefault(),

                                                    ObjAdditionalDetails = (from t in _context.Toppings
                                                                            join idtl in _context.OrderDetailAdditionalDetails
                                                                            on new { x1 = t.Id, x2 = (int)Helpers.Enums.ReferenceSection.Toppings, x3 = n.id } equals new { x1 = idtl.ReferenceId, x2 = idtl.ReferenceTypeId, x3 = idtl.OrderDetailId }
                                                                            into newidtl
                                                                            from idtl in newidtl.DefaultIfEmpty()
                                                                            where t.ItemId == _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.ItemId).FirstOrDefault()
                                                                                    && t.ItemSizeId == _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.ItemSizeId).FirstOrDefault()

                                                                            select new OrderDetailAdditionalDetailsDto
                                                                            {
                                                                                Id = idtl != null ? idtl.Id : 0,
                                                                                ReferenceId = idtl != null ? idtl.ReferenceId : t.Id,
                                                                                Name = t.Name,
                                                                                Price = idtl != null ? t.Price : t.Price,
                                                                                OrderDetailId = idtl != null ? idtl.OrderDetailId : 0,
                                                                                ReferenceTypeId = idtl != null ? idtl.ReferenceTypeId : 0,
                                                                                IsSelected = idtl != null ? true : false,

                                                                            }).ToList(),

                                                    Quantity = n.billGroup == 0 ? _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.Quantity).FirstOrDefault() :
                                                                _context.OrderDetail.Where(x => x.DealId.Equals(n.dealId) && x.BillGroup.Equals(n.billGroup)).Select(x => x.Quantity).FirstOrDefault(),

                                                    OrderType = n.billGroup == 0 ? ((Helpers.Enums.OrderType)_context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.OrderType).FirstOrDefault()).ToString() :
                                                                ((Helpers.Enums.OrderType)_context.OrderDetail.Where(x => x.DealId.Equals(n.dealId) && x.BillGroup.Equals(n.billGroup)).Select(x => x.OrderType).FirstOrDefault()).ToString(),

                                                    SubTotal = _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.SubTotal).FirstOrDefault(),
                                                }).ToList(),

                        CreatedById = data.CretedById,
                        DateCreated = data.DateCreated,
                        UpdatedById = data.UpdateById,
                        DateModified = data.DateModified,
                    };


                    foreach (var item in record.ObjGetAllOrderDetail)
                    {
                        if (item.DealId == null)
                        {
                            item.SubTotal = (item.ItemSizePrice.Value * item.Quantity) + ((item.ObjAdditionalDetails.Where(x => x.IsSelected == true).Sum(x => x.Price)) * item.Quantity);
                        }
                        else
                        {
                            item.SubTotal = (item.DealPrice.Value * item.Quantity);
                        }

                    };

                    //var grpprice = from g in record.ObjGetAllOrderDetail
                    //               where g.DealId != null
                    //               group g by new
                    //               {
                    //                   g.OrderId,
                    //                   g.DealId,
                    //                   g.BillGroup,
                    //                   g.DealPrice
                    //               } into gcs
                    //               select new
                    //               {
                    //                   price = gcs.Key.DealPrice
                    //               };

                    record.TotalAmount = record.ObjGetAllOrderDetail.Sum(x => x.SubTotal) + (data.DeliveryCharges == null ? 0 : data.DeliveryCharges.Value);

                    mainList.Add(record);
                }

                mainList = mainList.OrderByDescending(x => x.Id).ToList();

                _serviceResponse.Data = new { mainList };
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
        public async Task<ServiceResponse<object>> GetPaymentType()
        {

            var list = Enum.GetValues(typeof(Helpers.Enums.PaymentType))
               .Cast<Helpers.Enums.PaymentType>()
               .Select(t => new GetPaymentTypeDto
               {
                   Id = ((int)t),
                   Name = t.ToString(),
                   AccountNumber = "03001234567",
               });
            var toReturn = list;
            _serviceResponse.Data = toReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        //public async Task<ServiceResponse<object>> GetPaymentType()
        //{

        //        var objPaymentType = new GetPaymentTypeDto
        //        {
        //            PaymentType = Helpers.Enums.PaymentType,
        //        };

        //        _serviceResponse.Data = objPaymentType;
        //    _serviceResponse.Success = true;


        //    return _serviceResponse;

        //}

        public async Task<ServiceResponse<object>> ProcessOrder(AddProcessOrderDto dtoData)
        {
            var objCheckOutDetail = await _context.Orders.FirstOrDefaultAsync(s => s.Id.Equals(dtoData.OrderId));

            if (objCheckOutDetail != null)
            {

                var objOrderTransaction = new OrderTransaction
                {
                    OrderId = objCheckOutDetail.Id,
                    OrderStatusOld = objCheckOutDetail.OrderStatus,
                    CurrentStatus = dtoData.OrderStatus,
                    TransactionDate = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                    Lat = dtoData.Lat,
                    Long= dtoData.Long
                };
                _context.OrderTransaction.Add(objOrderTransaction);
                await _context.SaveChangesAsync();


                if (dtoData.SecoundaryAddress != null)
                {
                    var objSecoundaryAddress = new UserDeliveryAddress
                    {
                        UserId = _LoggedIn_UserID,
                        SecoundaryAddress = dtoData.SecoundaryAddress,
                    };
                    _context.UserDeliveryAddress.Update(objSecoundaryAddress);
                    await _context.SaveChangesAsync();
                }


                objCheckOutDetail.DeliveryAddress = dtoData.SecoundaryAddress == null ? objCheckOutDetail.ObjUser.Address : dtoData.SecoundaryAddress;
                objCheckOutDetail.Instructions = dtoData.Instructions;
                objCheckOutDetail.PaymentMethodType = ((Helpers.Enums.PaymentType)dtoData.PaymentMethodType).ToString();
                //objCheckOutDetail.DeliveryAddress = dtoData.DeliveryAddress;
                objCheckOutDetail.OrderStatus = dtoData.OrderStatus;

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
            var objcartlist = await _context.Orders.Where(x => x.UserId.Equals(_LoggedIn_UserID) 
            && x.OrderStatus == (int)Helpers.Enums.OrderStatus.PreOrder).ToListAsync();

            //var itemdtl = _context.OrderDetail.Where(x => x.ItemId == 5).ToList();

            if (objcartlist != null )
            {
                List<GetOrderByIdDto> data1 = new List<GetOrderByIdDto>();
                foreach (var item in objcartlist)
                {


                    var ObjOrder = new GetOrderByIdDto
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        UserName = _context.Users.Where(x => x.Id == item.UserId).Select(x => x.FullName).FirstOrDefault(),
                        //UserName = item.ObjUser == null ? string.Empty : item.ObjUser.FullName,
                        ContactNumber = _context.Users.Where(x => x.Id == item.UserId).Select(x => x.ContactNumber).FirstOrDefault(),
                        //ContactNumber = item.ObjUser == null ? string.Empty : item.ObjUser.ContactNumber,
                        OrderStatus = ((Helpers.Enums.OrderStatus)item.OrderStatus).ToString(),
                        PaymentMethodType = item.PaymentMethodType,
                        DeliveryAddress = _context.Users.Where(x => x.Id == item.UserId).Select(x => x.Address).FirstOrDefault(),
                        //DeliveryAddress = item.ObjUser == null ? string.Empty : item.ObjUser.Address,
                        EstimatedDeliveryTime = item.EstimatedDeliveryTime,
                        DeliveryCharges = item.DeliveryCharges,

                        CompanyId = item.CompanyId,
                        CompanyName = _context.Company.Where(x => x.Id == item.CompanyId).Select(x => x.Name).FirstOrDefault(),


                        CreatedById = item.CretedById,
                        DateCreated = item.DateCreated,
                        UpdatedById = item.UpdateById,
                        DateModified = item.DateModified,
                    };
                    var objOrderdtls = (from odtl in _context.OrderDetail
                                        where odtl.OrderId == item.Id
                                        group odtl by new
                                        {
                                            Id = odtl.DealId == null ? odtl.Id : 0,
                                            odtl.OrderId,
                                            odtl.DealId,
                                            odtl.BillGroup
                                        } into gcs
                                        select new
                                        {
                                            id = gcs.Key.Id,
                                            orderId = gcs.Key.OrderId,
                                            dealId = gcs.Key.DealId,
                                            billGroup = gcs.Key.BillGroup
                                        }).ToList();
                   
                  var   data = new GetOrderByIdDto
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        UserName = _context.Users.Where(x => x.Id == item.UserId).Select(x => x.FullName).FirstOrDefault(),
                        //UserName = item.ObjUser == null ? string.Empty : item.ObjUser.FullName,
                        ContactNumber = _context.Users.Where(x => x.Id == item.UserId).Select(x => x.ContactNumber).FirstOrDefault(),
                        //ContactNumber = item.ObjUser == null ? string.Empty : item.ObjUser.ContactNumber,
                        OrderStatus = ((Helpers.Enums.OrderStatus)item.OrderStatus).ToString(),
                        PaymentMethodType = item.PaymentMethodType,
                        DeliveryAddress = _context.Users.Where(x => x.Id == item.UserId).Select(x => x.Address).FirstOrDefault(),
                        //DeliveryAddress = item.ObjUser == null ? string.Empty : item.ObjUser.Address,
                        EstimatedDeliveryTime = item.EstimatedDeliveryTime,
                        DeliveryCharges = item.DeliveryCharges,

                        CompanyId = item.CompanyId,
                        CompanyName = _context.Company.Where(x => x.Id == item.CompanyId).Select(x => x.Name).FirstOrDefault(),

                      ObjGetAllOrderDetail = (from n in objOrderdtls
                                                let odtlData = from idtl in _context.OrderDetail
                                                               where n.dealId == null && idtl.Id == n.id
                                                               select idtl
                                                
                                                select new GetAllOrderDetailDto
                                                {
                                                    Id = n.id,
                                                    OrderId = item.Id,
                                                    DealId = n.dealId,
                                                    BillGroup = n.billGroup,
                                                    objDeals = (from odldeal in _context.OrderDetail
                                                                where odldeal.OrderId == item.Id && (odldeal.DealId == (n.dealId == null ? 0 : n.dealId)) && odldeal.BillGroup == n.billGroup
                                                                select new DealDto
                                                                {
                                                                    OrderDetailId = odldeal.Id,
                                                                    dealId = odldeal.DealId,
                                                                    billGroup = odldeal.BillGroup,
                                                                    itemId = odldeal.ItemId,
                                                                    Name = odldeal.ObjItem.Name
                                                                }).ToList(),

                                                    DealName = _context.Deals.Where(x => x.Id == n.dealId).Select(x => x.Title).FirstOrDefault(),
                                                    DealPrice = _context.Deals.Where(x => x.Id == n.dealId).Select(x => x.Price).FirstOrDefault(),
                                                    //CategoryId = n.CategoryId,
                                                    //CategoryName = p.Name,
                                                    ItemId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemId).FirstOrDefault(),
                                                    //ItemId = n.dealId == null ? _context.OrderDetail.Where(x => x.OrderId.Equals(odtlData.OrderId) && x.DealId == null).Select(x => x.ItemId).FirstOrDefault() :
                                                    //odtlData.ItemId,
                                                    ItemName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ObjItem.Name).FirstOrDefault(),
                                                    //ItemName = n.dealId == null ? odtlData.ItemId == null ? string.Empty :
                                                    //          _context.OrderDetail.Where(x => x.OrderId.Equals(odtlData.OrderId) && x.DealId == null).Select(x => x.ObjItem.Name).FirstOrDefault() :
                                                    //           odtlData.ItemId == null ? string.Empty : _context.Items.Where(x => x.Id == odtlData.ItemId).Select(x => x.Name).FirstOrDefault(),
                                                    ItemSizeId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? 0 : x.ItemSizeId).FirstOrDefault(),
                                                    //ItemSizeId = n.dealId == null ? _context.OrderDetail.Where(x => x.OrderId.Equals(odtlData.OrderId) && x.DealId == null).Select(x => x.ItemSizeId).FirstOrDefault() : 0,
                                                    ItemSizeName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? string.Empty : x.ObjItemSize.SizeDescription).FirstOrDefault(),
                                                    //ItemSizeName = n.dealId == null ? odtlData.ItemSizeId == null ? string.Empty :
                                                    //                _context.OrderDetail.Where(x => x.OrderId.Equals(odtlData.OrderId) && x.DealId == null).Select(x => x.ObjItemSize.SizeDescription).FirstOrDefault() : null,

                                                    ItemSizePrice = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? 0 : x.ObjItemSize.Price).FirstOrDefault(),

                                                    CrustId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? 0 : x.CrustId).FirstOrDefault(),
                                                    //CrustName = odtlData.CrustId == null ? string.Empty : _context.Crusts.Where(x => x.Id == odtlData.CrustId).Select(x => x.Name).FirstOrDefault(),
                                                    CrustName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? string.Empty : x.ObjCrust.Name).FirstOrDefault(),
                                                    //CrustPrice = odtlData.CrustId == null ? 0 : _context.Crusts.Where(x => x.Id == odtlData.CrustId).Select(x => x.Price).FirstOrDefault(),
                                                    CrustPrice = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? 0 : x.ObjCrust.Price).FirstOrDefault(),

                                                    ObjAdditionalDetails = (from t in _context.Toppings
                                                                            join idtl in _context.OrderDetailAdditionalDetails
                                                                            on new { x1 = t.Id, x2 = (int)Helpers.Enums.ReferenceSection.Toppings, x3 = n.id } equals new { x1 = idtl.ReferenceId, x2 = idtl.ReferenceTypeId, x3 = idtl.OrderDetailId }
                                                                            into newidtl
                                                                            from idtl in newidtl.DefaultIfEmpty()
                                                                            where t.ItemId == _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.ItemId).FirstOrDefault()
                                                                                    && t.ItemSizeId == _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.ItemSizeId).FirstOrDefault()

                                                                            //let data = (from idtl in _context.OrderDetailAdditionalDetails
                                                                            //            where idtl.ReferenceId == t.Id && idtl.ReferenceTypeId == (int)Helpers.Enums.ReferenceSection.Toppings
                                                                            //            select idtl).ToList()

                                                                            select new OrderDetailAdditionalDetailsDto
                                                                            {
                                                                                Id = idtl != null ? idtl.Id : 0,
                                                                                ReferenceId = idtl != null ? idtl.ReferenceId : t.Id,
                                                                                Name = t.Name,
                                                                                Price = idtl != null ? t.Price : t.Price,
                                                                                OrderDetailId = idtl != null ? idtl.OrderDetailId : 0,
                                                                                ReferenceTypeId = idtl != null ? idtl.ReferenceTypeId : 0,
                                                                                IsSelected = idtl != null ? true : false,

                                                                            }).ToList(),

                                                    Quantity = n.billGroup == 0 ? _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.Quantity).FirstOrDefault() :
                                                                _context.OrderDetail.Where(x => x.OrderId == n.orderId &&  x.DealId.Equals(n.dealId) && x.BillGroup.Equals(n.billGroup)).Select(x => x.Quantity).FirstOrDefault(),

                                                    OrderType = n.billGroup == 0 ? ((Helpers.Enums.OrderType)_context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.OrderType).FirstOrDefault()).ToString() :
                                                                ((Helpers.Enums.OrderType)_context.OrderDetail.Where(x => x.OrderId == n.orderId &&  x.DealId.Equals(n.dealId) && x.BillGroup.Equals(n.billGroup)).Select(x => x.OrderType).FirstOrDefault()).ToString(),

                                                    SubTotal = _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.SubTotal).FirstOrDefault(),
                                                }).ToList(),

                        CreatedById = item.CretedById,
                        DateCreated = item.DateCreated,
                        UpdatedById = item.UpdateById,
                        DateModified = item.DateModified,
                    };

                    data1.Add(data);

                    //TotalAmount = ObjGetAllOrderDetail.Sum(n => Convert.ToDecimal(n.SubTotal, CultureInfo.InvariantCulture)),

                    foreach (var i in data.ObjGetAllOrderDetail)
                    {
                        if (i.DealId == null)
                        {
                            i.SubTotal = (i.ItemSizePrice.Value * i.Quantity) + ((i.ObjAdditionalDetails.Where(x => x.IsSelected == true).Sum(x => x.Price)) * i.Quantity);
                        }
                        else
                        {
                            i.SubTotal = (i.DealPrice.Value * i.Quantity);
                        }
                    };

                    

                    data.TotalAmount = data.ObjGetAllOrderDetail.Sum(x => x.SubTotal) + (data.DeliveryCharges == null ? 0 : data.DeliveryCharges.Value);

                    _serviceResponse.Data = data1;
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = "Record Found";
                };
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            };

            return _serviceResponse;
        }


        public async Task<ServiceResponse<object>> GetOrderForRider(int CompanyId)
        {

            List<GetAllOrderDto> mainList = new List<GetAllOrderDto>();


            var list = await _context.Orders.Where(x => x.RiderId == _LoggedIn_UserID && x.CompanyId == CompanyId &&  x.OrderStatus == (int)Helpers.Enums.OrderStatus.ReadyToDeliver
                                          || x.OrderStatus == (int)Helpers.Enums.OrderStatus.OnTheWay
                                          || x.OrderStatus == (int)Helpers.Enums.OrderStatus.Delivered).ToListAsync();
            if (list != null)
            {
                foreach (var data in list)
                {

                    var record = new GetAllOrderDto
                    {
                        Id = data.Id,
                        UserId = data.UserId,
                        UserName = data.ObjUser == null ? string.Empty : data.ObjUser.FullName,
                        OrderStatus = ((Helpers.Enums.OrderStatus)data.OrderStatus).ToString(),
                        PaymentMethodType = data.PaymentMethodType,
                        TotalAmount = data.TotalAmount,
                        DeliveryAddress = data.DeliveryAddress,
                        DeliveryCharges = data.DeliveryCharges,
                        EstimatedDeliveryTime = data.EstimatedDeliveryTime,
                        Instructions = data.Instructions,
                        CompanyId = data.CompanyId,
                        DateCreated = data.DateCreated,

                        ObjGetAllOrderDetail = (from n in _context.OrderDetail
                                                where n.OrderId == data.Id

                                                select new GetAllOrderDetailDto
                                                {
                                                    Id = n.Id,
                                                    OrderId = data.Id,
                                                    DealId = n.DealId,  //(n.DealId > 0 ? n.DealId : null),
                                                    DealName = n.ObjDeal.Title,
                                                    DealPrice = n.ObjDeal.Price,
                                                    //CategoryId = n.CategoryId,
                                                    //CategoryName = n.ObjCategory.Name,
                                                    ItemId = n.ItemId,
                                                    ItemName = n.ObjItem == null ? string.Empty : n.ObjItem.Name,
                                                    ItemSizeId = n.ItemSizeId,
                                                    ItemSizeName = n.ObjItemSize.SizeDescription,
                                                    ItemSizePrice = n.ObjItemSize.Price,
                                                    CrustId = n.CrustId,
                                                    CrustName = n.ObjCrust.Name,
                                                    CrustPrice = n.ObjCrust.Price,

                                                    ObjAdditionalDetails = (from t in _context.Toppings
                                                                            join idtl in _context.OrderDetailAdditionalDetails
                                                                            on new { x1 = t.Id, x2 = (int)Helpers.Enums.ReferenceSection.Toppings, x3 = n.Id } equals new { x1 = idtl.ReferenceId, x2 = idtl.ReferenceTypeId, x3 = idtl.OrderDetailId }
                                                                            into newidtl
                                                                            from idtl in newidtl.DefaultIfEmpty()
                                                                            where t.ItemId == n.ItemId && t.ItemSizeId == n.ItemSizeId

                                                                            //let data = (from idtl in _context.OrderDetailAdditionalDetails
                                                                            //            where idtl.ReferenceId == t.Id && idtl.ReferenceTypeId == (int)Helpers.Enums.ReferenceSection.Toppings
                                                                            //            select idtl).ToList()

                                                                            select new OrderDetailAdditionalDetailsDto
                                                                            {
                                                                                Id = idtl != null ? idtl.Id : 0,
                                                                                ReferenceId = idtl != null ? idtl.ReferenceId : t.Id,
                                                                                Name = t.Name,
                                                                                Price = idtl != null ? t.Price : t.Price,
                                                                                OrderDetailId = idtl != null ? idtl.OrderDetailId : 0,
                                                                                ReferenceTypeId = idtl != null ? idtl.ReferenceTypeId : 0,
                                                                                IsSelected = idtl != null ? true : false,

                                                                            }).ToList(),

                                                    Quantity = n.Quantity,
                                                    //CompanyId = n.CompanyId,
                                                    OrderType = ((Helpers.Enums.OrderType)n.OrderType).ToString(),
                                                    //Instructions = n.Instructions,
                                                    BillGroup = n.BillGroup,
                                                }).ToList(),



                        CreatedById = data.CretedById,
                        UpdatedById = data.UpdateById,
                        DateModified = data.DateModified,
                    };

                    foreach (var item in record.ObjGetAllOrderDetail)
                    {
                        if (item.DealId == null)
                        {
                            item.SubTotal = (item.ItemSizePrice.Value * item.Quantity) + ((item.ObjAdditionalDetails.Where(x => x.IsSelected == true).Sum(x => x.Price)) * item.Quantity);
                        }
                        else
                        {
                            item.SubTotal = item.DealPrice.Value;
                        }

                    };

                    var grpprice = from g in record.ObjGetAllOrderDetail
                                   where g.DealId != null
                                   group g by new
                                   {
                                       g.OrderId,
                                       g.DealId,
                                       g.BillGroup,
                                       g.DealPrice
                                   } into gcs
                                   select new
                                   {
                                       price = gcs.Key.DealPrice
                                   };

                    record.TotalAmount = record.ObjGetAllOrderDetail.Where(x => x.DealId == null).Sum(x => x.SubTotal) + grpprice.Sum(x => x.price.Value) + (data.DeliveryCharges == null ? 0 : data.DeliveryCharges.Value);

                    mainList.Add(record);
                }
                _serviceResponse.Data = new { mainList };
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


        public async Task<ServiceResponse<object>> RiderStuatusUpdate(AddProcessOrderDto dtoData)
        {
            var objRiderStatus = await _context.Orders.FirstOrDefaultAsync(s => s.Id.Equals(dtoData.OrderId));

            if (objRiderStatus != null)
            {
                var objOrderTransaction = new OrderTransaction
                {
                    OrderId = objRiderStatus.Id,
                    OrderStatusOld = objRiderStatus.OrderStatus,
                    CurrentStatus = dtoData.OrderStatus,
                    TransactionDate = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };
                _context.OrderTransaction.Add(objOrderTransaction);
                await _context.SaveChangesAsync();


                objRiderStatus.PaymentMethodType = ((Helpers.Enums.PaymentType)dtoData.PaymentMethodType).ToString();
                //objCheckOutDetail.DeliveryAddress = dtoData.DeliveryAddress;
                objRiderStatus.OrderStatus = dtoData.OrderStatus;

                _context.Orders.Update(objRiderStatus);
                await _context.SaveChangesAsync();

                //history data record insert

                var OrderStatusTransactionobj = new OrderStatusTransaction
                {
                    OrderId = objRiderStatus.Id,
                    OrderStatus = objRiderStatus.OrderStatus,
                    ActiveQueue = (int)Helpers.Enums.OrderStatus.Delivered == 6 ? false : true,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                    RiderId = objRiderStatus.RiderId.ToString(),
                };

                await _context.OrderStatusTransaction.AddAsync(OrderStatusTransactionobj);
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
        public async Task<ServiceResponse<object>> GetOrderByCompany(int CompanyId, int orderStatus, int page, int pageSize)
        {

            List<GetAllOrderDto> mainList = new List<GetAllOrderDto>();



            var listCount = _context.Orders.Where(x => CompanyId > 0 ? x.CompanyId.Equals(CompanyId) : x.UserId == _LoggedIn_UserID &&
           orderStatus == (int)Helpers.Enums.OrderStatus.All ? x.OrderStatus != (int)Helpers.Enums.OrderStatus.PreOrder : x.OrderStatus == orderStatus).Count();

            var skip = pageSize * (page - 1);
            //var canPage = skip < listCount;

            var list = await _context.Orders.Where(x => CompanyId > 0 ? x.CompanyId.Equals(CompanyId) : x.UserId == _LoggedIn_UserID &&
            orderStatus == (int)Helpers.Enums.OrderStatus.All ? x.OrderStatus != (int)Helpers.Enums.OrderStatus.PreOrder : x.OrderStatus == orderStatus).Skip(skip).Take(pageSize).ToListAsync();

            if (list != null)
            {
                foreach (var data in list)
                {
                    var objOrderdtls = (from odtl in _context.OrderDetail
                                        where odtl.OrderId == data.Id
                                        group odtl by new
                                        {
                                            Id = odtl.DealId == null ? odtl.Id : 0,
                                            odtl.OrderId,
                                            odtl.DealId,
                                            odtl.BillGroup
                                        } into gcs
                                        select new
                                        {
                                            id = gcs.Key.Id,
                                            orderId = gcs.Key.OrderId,
                                            dealId = gcs.Key.DealId,
                                            billGroup = gcs.Key.BillGroup
                                        }).ToList();

                    var record = new GetAllOrderDto
                    {
                        Id = data.Id,
                        UserId = data.UserId,
                        UserName = _context.Users.Where(x => x.Id == data.UserId).Select(x => x.FullName).FirstOrDefault(),
                        OrderStatus = ((Helpers.Enums.OrderStatus)data.OrderStatus).ToString(),
                        PaymentMethodType = data.PaymentMethodType,
                        DeliveryAddress = _context.Users.Where(x => x.Id == data.UserId).Select(x => x.Address).FirstOrDefault(),
                        EstimatedDeliveryTime = data.EstimatedDeliveryTime,
                        DeliveryCharges = data.DeliveryCharges,
                        //CompanyName = data.ObjCompany.Name,

                        ObjGetAllOrderDetail = (from n in objOrderdtls
                                                let odtlData = from idtl in _context.OrderDetail
                                                               where n.dealId == null && idtl.Id == n.id
                                                               select idtl

                                                select new GetAllOrderDetailDto
                                                {
                                                    Id = n.id,
                                                    OrderId = data.Id,
                                                    DealId = n.dealId,
                                                    BillGroup = n.billGroup,
                                                    objDeals = (from odldeal in _context.OrderDetail
                                                                where odldeal.OrderId == data.Id && (odldeal.DealId == (n.dealId == null ? 0 : n.dealId)) && odldeal.BillGroup == n.billGroup
                                                                select new DealDto
                                                                {
                                                                    OrderDetailId = odldeal.Id,
                                                                    dealId = odldeal.DealId,
                                                                    billGroup = odldeal.BillGroup,
                                                                    itemId = odldeal.ItemId,
                                                                    Name = odldeal.ObjItem.Name
                                                                }).ToList(),

                                                    DealName = _context.Deals.Where(x => x.Id == n.dealId).Select(x => x.Title).FirstOrDefault(),
                                                    DealPrice = _context.Deals.Where(x => x.Id == n.dealId).Select(x => x.Price).FirstOrDefault(),
                                                    ItemId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemId).FirstOrDefault(),
                                                    ItemName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ObjItem.Name).FirstOrDefault(),
                                                    ItemSizeId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? 0 : x.ItemSizeId).FirstOrDefault(),
                                                    ItemSizeName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? string.Empty : x.ObjItemSize.SizeDescription).FirstOrDefault(),
                                                    ItemSizePrice = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.ItemSizeId == null ? 0 : x.ObjItemSize.Price).FirstOrDefault(),
                                                    CrustId = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? 0 : x.CrustId).FirstOrDefault(),
                                                    CrustName = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? string.Empty : x.ObjCrust.Name).FirstOrDefault(),
                                                    CrustPrice = _context.OrderDetail.Where(x => x.Id.Equals(n.id) && x.DealId == null).Select(x => x.CrustId == null ? 0 : x.ObjCrust.Price).FirstOrDefault(),

                                                    ObjAdditionalDetails = (from t in _context.Toppings
                                                                            join idtl in _context.OrderDetailAdditionalDetails
                                                                            on new { x1 = t.Id, x2 = (int)Helpers.Enums.ReferenceSection.Toppings, x3 = n.id } equals new { x1 = idtl.ReferenceId, x2 = idtl.ReferenceTypeId, x3 = idtl.OrderDetailId }
                                                                            into newidtl
                                                                            from idtl in newidtl.DefaultIfEmpty()
                                                                            where t.ItemId == _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.ItemId).FirstOrDefault()
                                                                                    && t.ItemSizeId == _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.ItemSizeId).FirstOrDefault()

                                                                            select new OrderDetailAdditionalDetailsDto
                                                                            {
                                                                                Id = idtl != null ? idtl.Id : 0,
                                                                                ReferenceId = idtl != null ? idtl.ReferenceId : t.Id,
                                                                                Name = t.Name,
                                                                                Price = idtl != null ? t.Price : t.Price,
                                                                                OrderDetailId = idtl != null ? idtl.OrderDetailId : 0,
                                                                                ReferenceTypeId = idtl != null ? idtl.ReferenceTypeId : 0,
                                                                                IsSelected = idtl != null ? true : false,

                                                                            }).ToList(),

                                                    Quantity = n.billGroup == 0 ? _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.Quantity).FirstOrDefault() :
                                                                _context.OrderDetail.Where(x => x.DealId.Equals(n.dealId) && x.BillGroup.Equals(n.billGroup)).Select(x => x.Quantity).FirstOrDefault(),

                                                    OrderType = n.billGroup == 0 ? ((Helpers.Enums.OrderType)_context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.OrderType).FirstOrDefault()).ToString() :
                                                                ((Helpers.Enums.OrderType)_context.OrderDetail.Where(x => x.DealId.Equals(n.dealId) && x.BillGroup.Equals(n.billGroup)).Select(x => x.OrderType).FirstOrDefault()).ToString(),

                                                    SubTotal = _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.SubTotal).FirstOrDefault(),
                                                }).ToList(),

                        CreatedById = data.CretedById,
                        DateCreated = data.DateCreated,
                        UpdatedById = data.UpdateById,
                        DateModified = data.DateModified,
                    };


                    foreach (var item in record.ObjGetAllOrderDetail)
                    {
                        if (item.DealId == null)
                        {
                            item.SubTotal = (item.ItemSizePrice.Value * item.Quantity) + ((item.ObjAdditionalDetails.Where(x => x.IsSelected == true).Sum(x => x.Price)) * item.Quantity);
                        }
                        else
                        {
                            item.SubTotal = (item.DealPrice.Value * item.Quantity);
                        }

                    };

                    //var grpprice = from g in record.ObjGetAllOrderDetail
                    //               where g.DealId != null
                    //               group g by new
                    //               {
                    //                   g.OrderId,
                    //                   g.DealId,
                    //                   g.BillGroup,
                    //                   g.DealPrice
                    //               } into gcs
                    //               select new
                    //               {
                    //                   price = gcs.Key.DealPrice
                    //               };

                    record.TotalAmount = record.ObjGetAllOrderDetail.Sum(x => x.SubTotal) + (data.DeliveryCharges == null ? 0 : data.DeliveryCharges.Value);

                    mainList.Add(record);
                }

                mainList = mainList.OrderByDescending(x => x.Id).ToList();

                _serviceResponse.Data = new { mainList };
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
