using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
using System.Security.Cryptography;
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
            int Id = _context.Orders.Max(x => x.Id) + 1;
            string idstr = "00000" + Convert.ToString(Id);
            var OrderToCreate = new Order
            {
                OrderStatus = dtoData.OrderStatus,
                CompanyId = dtoData.CompanyId,
                CretedById = _LoggedIn_UserID,
                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                //OrderNumber = DbFunctions.Right("00000" +Id, 6),
                OrderNumber= idstr.Substring(idstr.Length-6)
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
                objorder.UserId =dtoData.UserId;
                objorder.TotalAmount =dtoData.Amount;
                objorder.DeliveryCharges =dtoData.DeliveryCharges;
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

                                                    OrderType = /*n.billGroup == 0 ?*/ ((Helpers.Enums.OrderType)_context.Orders.Where(x => x.Id.Equals(data.Id)).Select(x => x.OrderType).FirstOrDefault()).ToString(),
                                                    //:
                                                    //            ((Helpers.Enums.OrderType)_context.Orders.Where(x => x.DealId.Equals(n.dealId) && x.BillGroup.Equals(n.billGroup)).Select(x => x.OrderType).FirstOrDefault()).ToString(),

                                                    //SubTotal = _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.SubTotal).FirstOrDefault(),
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
            string orderNo = await _context.Orders.Where(x=>x.CompanyId.Equals(objCheckOutDetail.CompanyId)).MaxAsync(s => s.OrderNumber);

            string OrdNumb = orderNo==null?"00000" + 1 : "00000" + (Convert.ToInt32(orderNo)+1);
           
            if (objCheckOutDetail != null)
            {

                var objOrderTransaction = new OrderTransaction
                {
                    OrderId = objCheckOutDetail.Id,
                    OrderStatusOld = objCheckOutDetail.OrderStatus,
                    CurrentStatus = dtoData.OrderStatus,
                    TransactionDate = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                    Lat = dtoData.Lat,
                    Long = dtoData.Long
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
                objCheckOutDetail.OrderNumber= OrdNumb.Substring(OrdNumb.Length-6);

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
            var objGetCartList = await (from o in _context.Orders
                                        where o.UserId == _LoggedIn_UserID && o.OrderStatus == (int)Helpers.Enums.OrderStatus.PreOrder
                                        let dealIds = _context.OrderDetail.Where(x => x.OrderId == o.Id && x.DealId != null).GroupBy(x => x.DealId).Select(g => new
                                        {
                                            Id = g.Max(row => row.Id)
                                        }).Select(x => x.Id.ToString()).ToList()                                                  
            select new GetOrderByIdDto
                                        {
                                            Id = o.Id,
                                            UserId = o.UserId,
                                            UserName = o.ObjUser.FullName,
                                            ContactNumber = o.ObjUser.ContactNumber,
                                            CompanyId = o.CompanyId,
                                            CompanyName = o.ObjCompany.Name,
                                            OrderStatus = ((Helpers.Enums.OrderStatus)o.OrderStatus).ToString(),
                                            PaymentMethodType = o.PaymentMethodType,
                                            DeliveryAddress = o.ObjUser.Address,
                                            DeliveryCharges = o.DeliveryCharges,
                                            ObjGetAllOrderDetail = (from odtl in _context.OrderDetail
                                                                    where (odtl.OrderId == o.Id && odtl.DealId == null)
                                                                    || (dealIds.Contains(odtl.Id.ToString())
                                                                    )
                                                                    select new GetAllOrderDetailDto
                                                                    {
                                                                        Id = odtl.Id,
                                                                        DealId = odtl.DealId,
                                                                        BillGroup = odtl.BillGroup,
                                                                        objDeals = (from odldeal in _context.OrderDetail
                                                                                    where odldeal.OrderId == o.Id && (odldeal.DealId == (odtl.DealId == null ? 0 : odtl.DealId)) && odldeal.BillGroup == odtl.BillGroup
                                                                                    select new DealDto
                                                                                    {
                                                                                        OrderDetailId = odldeal.Id,
                                                                                        dealId = odldeal.DealId,
                                                                                        billGroup = odldeal.BillGroup,
                                                                                        itemId = odldeal.ItemId,
                                                                                        Name = odldeal.ObjItem.Name,
                                                                                        FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + odldeal.ObjDeal.FilePath + '/' + odldeal.ObjDeal.FileName
                                                                                    }).ToList(),
                                                                        DealName = odtl.ObjDeal.Title,
                                                                        DealPrice = odtl.ObjDeal.Price,
                                                                        ItemId = odtl.ItemId,
                                                                        ItemName = odtl.DealId != null ? string.Empty : odtl.ObjItem.Name,
                                                                        ItemSizeId = odtl.ItemSizeId,
                                                                        ItemSizeName = odtl.ObjItemSize.SizeDescription,
                                                                        FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + odtl.ObjItem.FilePath + '/' + odtl.ObjItem.FileName,
                                                                        ItemSizePrice = odtl.ObjItemSize.Price>0? odtl.ObjItemSize.Price:odtl.ObjItem.Price,
                                                                        CrustId = odtl.ObjCrust.Id,
                                                                        CrustName = odtl.ObjCrust.Name,
                                                                        CrustPrice = odtl.ObjCrust.Price,
                                                                        ObjAdditionalDetails = (from t in _context.Toppings
                                                                                                join idtl in _context.OrderDetailAdditionalDetails
                                                                                                on new { x1 = t.Id, x2 = (int)Helpers.Enums.ReferenceSection.Toppings, x3 = odtl.Id }
                                                                                                equals new { x1 = idtl.ReferenceId, x2 = idtl.ReferenceTypeId, x3 = idtl.OrderDetailId }
                                                                                                into newidtl
                                                                                                from idtl in newidtl.DefaultIfEmpty()
                                                                                                where odtl.ItemSizeId != null ? t.ItemId == odtl.ItemId && t.ItemSizeId == odtl.ItemSizeId : false

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
                                                                        Quantity = odtl.Quantity,
                                                                        OrderType = ((Helpers.Enums.OrderType)o.OrderType).ToString(),
                                                                        //SubTotal=odtl.DealId==null?(odtl.ObjItemSize.Price+odtl.Quantity)+(ObjAdditionalDetails.)
                                                                    }).ToList(),
                                            CreatedById = o.CretedById,
                                            DateCreated = o.DateCreated,
                                            UpdatedById = o.UpdateById,
                                            DateModified = o.DateModified,
                                         //TotalAmount = SubTotal.Select(x=> x.Price * x.Quantity).Sum()

                                        }).ToListAsync();

            foreach (var cart in objGetCartList)
            {
                foreach (var item in cart.ObjGetAllOrderDetail)
                {
                    item.SubTotal = item.DealId == null ? (item.ItemSizePrice.Value * item.Quantity) + ((item.ObjAdditionalDetails.Where(x => x.IsSelected == true).Sum(x => x.Price)) * item.Quantity) :
                               (item.DealDiscountPrice != null && item.DealDiscountPrice != 0) ? (item.DealDiscountPrice.Value * item.Quantity)
                               : (item.DealPrice.Value * item.Quantity);
                }

                cart.TotalAmount = cart.ObjGetAllOrderDetail.Sum(x => x.SubTotal) + (cart.DeliveryCharges ?? 0);
            };

            if (objGetCartList.Count>0)
            {           
                _serviceResponse.Data = objGetCartList;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
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


            var list = await _context.Orders.Where(x => x.RiderId == _LoggedIn_UserID && x.CompanyId == CompanyId && x.OrderStatus == (int)Helpers.Enums.OrderStatus.ReadyToDeliver
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
                                                    OrderType = ((Helpers.Enums.OrderType)data.OrderType).ToString(),
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

                                                    OrderType = /*n.billGroup == 0 ?*/ ((Helpers.Enums.OrderType)_context.Orders.Where(x => x.Id.Equals(data.Id)).Select(x => x.OrderType).FirstOrDefault()).ToString(), 
                                                    //:
                                                    //            ((Helpers.Enums.OrderType)_context.OrderDetail.Where(x => x.DealId.Equals(n.dealId) && x.BillGroup.Equals(n.billGroup)).Select(x => x.OrderType).FirstOrDefault()).ToString(),

                                                    //SubTotal = _context.OrderDetail.Where(x => x.Id.Equals(n.id)).Select(x => x.SubTotal).FirstOrDefault(),
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
        public async Task<ServiceResponse<object>> GetOrderDetailById(int Id)
        {

            var list =await (from ord in _context.Orders
                        join use in _context.Users on ord.UserId equals use.Id
                        join odtl in _context.OrderDetail on ord.Id equals odtl.OrderId
                        where ord.Id == Id
                        group new { ord, use, odtl } by new
                        {
                            Id = ord.Id,
                            UserName = use.FullName,
                            Status = ord.OrderStatus,//((Helpers.Enums.OrderStatus)ord.OrderStatus).ToString(),
                            OrderNumebr = ord.OrderNumber,
                            Amount = ord.TotalAmount,
                            DeliveryCharges = ord.DeliveryCharges ?? 0,
                            ItemId = (odtl.DealId == null ? odtl.ItemId.Value : 0),
                            UpdatedById = ord.UpdateById,
                            CompanyId=ord.CompanyId,
                            OderDetailId = odtl.DealId == null ? odtl.Id : 0,
                            OrderTypeId = ord.OrderType,
                            Quantity = odtl.DealId != null ? odtl.Quantity : 0,
                            DealId = odtl.DealId != null ? odtl.DealId.Value : 0,
                            BillGroup = odtl.DealId != null ? odtl.BillGroup : 0,

                                    } into grp

                        select new GetMasterOrderDetailListDto
                        {
                            Id = grp.Key.Id,
                            UserName = grp.Key.UserName,
                            Status = ((Helpers.Enums.OrderStatus)grp.Key.Status).ToString(),
                            OrderNumebr =grp.Key.OrderNumebr,
                            Amount = grp.Key.Amount,
                            ItemId = grp.Key.ItemId,
                            DealId = grp.Key.DealId,
                            BillGroup = grp.Key.BillGroup,
                            OderDetailId = grp.Key.OderDetailId,
                            OrderType = grp.Key.OrderTypeId??0,
                            CompanyId=grp.Key.CompanyId,
                            UpdatedBy=_context.Users.Where(a=>a.Id== grp.Key.UpdatedById).Select(x=>x.FullName).FirstOrDefault(),
                        
                        }
                      ).ToListAsync();


            int i = 0;

            foreach (var item in list)
            {               
                    item.ObjOrderDetail = (from odtl in _context.OrderDetail
                                           where odtl.OrderId == item.Id
                                          && (item.ItemId > 0 ? (odtl.ItemId == item.ItemId && odtl.BillGroup==0):
                                                        (odtl.DealId == item.DealId && odtl.BillGroup == item.BillGroup && odtl.Id ==
                                                        _context.OrderDetail.First(x => x.DealId == item.DealId && x.BillGroup == item.BillGroup).Id))
                                           select new GetOrderDetailListDto
                                           {
                                               Id = odtl.Id,
                                               OrderType= ((Helpers.Enums.OrderType)item.OrderType).ToString(),
                                               DealId = item.DealId,
                                               DealTitle = _context.Deals.Where(x => x.Id == item.DealId).Select(x => x.Title).FirstOrDefault(),
                                               ObjDealDetail = (from deal in _context.Deals
                                                                where deal.Id == odtl.DealId

                                                                select new GetDealListDto
                                                                {                                                                    
                                                                    ObjItemDetail = (from ODtls in _context.OrderDetail
                                                                                     join
                                                                                    dl in _context.Deals on ODtls.DealId equals
                                                                                    dl.Id
                                                                                     where ODtls.DealId == deal.Id

                                                                                     select new GetItemListDto
                                                                                     {
                                                                                         Id = ODtls.ItemId.Value,
                                                                                         Name = _context.Items.Where(x => x.Id == ODtls.ItemId.Value).Select(x => x.Name).FirstOrDefault(),
                                                                                         CategoryId = ODtls.ObjItem.CategoryId,
                                                                                         Quantity = ODtls.Quantity

                                                                                     }).ToList(),

                                                                }).ToList(),
                                               Quantity=odtl.Quantity,
                                               ItemId= odtl.ItemId.Value,
                                               ItemName= _context.Items.Where(x => x.Id == odtl.ItemId.Value && odtl.DealId==null).Select(x => x.Name).FirstOrDefault(),
                                               ItemSizeId = odtl.DealId == null ? odtl.ItemSizeId.Value : 0,
                                               ItemSizeName = odtl.DealId == null ? odtl.ObjItemSize.SizeDescription : "",
                                               ItemPrice = odtl.DealId == null ? odtl.ObjItemSize.Price>0? odtl.ObjItemSize.Price:odtl.ObjItem.Price : 0,
                                               IsDeal = item.DealId > 0 ? true : false
                                           }).ToList();              
            }           
           
            if(list.Count>0)
            {
                foreach (var item in list)
                {
                    i = i + 1;
                    if (i > 1)
                    {
                        list[0].ObjOrderDetail.AddRange(item.ObjOrderDetail);

                    }
                }
                list.RemoveRange(1, list.Count - 1);

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
        public async Task<ServiceResponse<object>> GetAllOrdersList()
        {
            var list =await (from ord in _context.Orders                     
                        select new GetAllOrdersList
                        {
                            Id=ord.Id,
                            Status= ((Helpers.Enums.OrderStatus)ord.OrderStatus).ToString(),
                            UserName=ord.ObjUser.FullName,
                            OrderNumebr=ord.OrderNumber,
                            DeliveryCharges=ord.DeliveryCharges??0,                           
                            UpdatedBy=ord.ObjUpdatedBy.FullName,                                                      
                            CompanyId=ord.CompanyId,                                                      
                             }
                        ).ToListAsync();
            foreach (var item in list)
            {
                item.Amount = Convert.ToInt32(GetCalculatedAmountByOrderId(item.Id,item.DeliveryCharges));
            }
               
            if (list != null)
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
        public async Task<ServiceResponse<object>> GetAllOrdersDetail()
        {
            var objOrdersList = await (from o in _context.Orders
                                        where o.UserId == _LoggedIn_UserID 
                                        let dealIds = _context.OrderDetail.Where(x => x.OrderId == o.Id && x.DealId != null).GroupBy(x => x.DealId).Select(g => new
                                        {
                                            Id = g.Max(row => row.Id)
                                        }).Select(x => x.Id.ToString()).ToList()
                                        select new GetOrderByIdDto
                                        {
                                            Id = o.Id,
                                            UserId = o.UserId,
                                            UserName = o.ObjUser.FullName,
                                            ContactNumber = o.ObjUser.ContactNumber,
                                            CompanyId = o.CompanyId,
                                            CompanyName = o.ObjCompany.Name,
                                            OrderStatus = ((Helpers.Enums.OrderStatus)o.OrderStatus).ToString(),
                                            PaymentMethodType = o.PaymentMethodType,
                                            DeliveryAddress = o.ObjUser.Address,
                                            DeliveryCharges = o.DeliveryCharges,
                                            ObjGetAllOrderDetail = (from odtl in _context.OrderDetail
                                                                    where (odtl.OrderId == o.Id && odtl.DealId == null)
                                                                    || (dealIds.Contains(odtl.Id.ToString())
                                                                    )
                                                                    select new GetAllOrderDetailDto
                                                                    {
                                                                        Id = odtl.Id,
                                                                        DealId = odtl.DealId,
                                                                        BillGroup = odtl.BillGroup,
                                                                        objDeals = (from odldeal in _context.OrderDetail
                                                                                    where odldeal.OrderId == o.Id && (odldeal.DealId == (odtl.DealId == null ? 0 : odtl.DealId)) && odldeal.BillGroup == odtl.BillGroup
                                                                                    select new DealDto
                                                                                    {
                                                                                        OrderDetailId = odldeal.Id,
                                                                                        dealId = odldeal.DealId,
                                                                                        billGroup = odldeal.BillGroup,
                                                                                        itemId = odldeal.ItemId,
                                                                                        Name = odldeal.ObjItem.Name,
                                                                                        FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + odldeal.ObjDeal.FilePath + '/' + odldeal.ObjDeal.FileName
                                                                                    }).ToList(),
                                                                        DealName = odtl.ObjDeal.Title,
                                                                        DealPrice = odtl.ObjDeal.Price,
                                                                        ItemId = odtl.ItemId,
                                                                        ItemName = odtl.DealId != null ? string.Empty : odtl.ObjItem.Name,
                                                                        ItemSizeId = odtl.ItemSizeId,
                                                                        ItemSizeName = odtl.ObjItemSize.SizeDescription,
                                                                        FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + odtl.ObjItem.FilePath + '/' + odtl.ObjItem.FileName,
                                                                        ItemSizePrice = odtl.ObjItemSize.Price > 0 ? odtl.ObjItemSize.Price : odtl.ObjItem.Price,
                                                                        CrustId = odtl.ObjCrust.Id,
                                                                        CrustName = odtl.ObjCrust.Name,
                                                                        CrustPrice = odtl.ObjCrust.Price,
                                                                        ObjAdditionalDetails = (from t in _context.Toppings
                                                                                                join idtl in _context.OrderDetailAdditionalDetails
                                                                                                on new { x1 = t.Id, x2 = (int)Helpers.Enums.ReferenceSection.Toppings, x3 = odtl.Id }
                                                                                                equals new { x1 = idtl.ReferenceId, x2 = idtl.ReferenceTypeId, x3 = idtl.OrderDetailId }
                                                                                                into newidtl
                                                                                                from idtl in newidtl.DefaultIfEmpty()
                                                                                                where odtl.ItemSizeId != null ? t.ItemId == odtl.ItemId && t.ItemSizeId == odtl.ItemSizeId : false

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
                                                                        Quantity = odtl.Quantity,
                                                                        OrderType = ((Helpers.Enums.OrderType)o.OrderType).ToString(),
                                                                        //SubTotal=odtl.DealId==null?(odtl.ObjItemSize.Price+odtl.Quantity)+(ObjAdditionalDetails.)
                                                                    }).ToList(),
                                            CreatedById = o.CretedById,
                                            DateCreated = o.DateCreated,
                                            UpdatedById = o.UpdateById,
                                            DateModified = o.DateModified,
                                            //TotalAmount = SubTotal.Select(x=> x.Price * x.Quantity).Sum()

                                        }).ToListAsync();

            foreach (var ord in objOrdersList)
            {
                foreach (var item in ord.ObjGetAllOrderDetail)
                {
                    item.SubTotal = item.DealId == null ? (item.ItemSizePrice.Value * item.Quantity) + ((item.ObjAdditionalDetails.Where(x => x.IsSelected == true).Sum(x => x.Price)) * item.Quantity) :
                               (item.DealDiscountPrice != null && item.DealDiscountPrice != 0) ? (item.DealDiscountPrice.Value * item.Quantity)
                               : (item.DealPrice.Value * item.Quantity);
                }

                ord.TotalAmount = ord.ObjGetAllOrderDetail.Sum(x => x.SubTotal) + (ord.DeliveryCharges ?? 0);
            };

            if (objOrdersList.Count > 0)
            {
                _serviceResponse.Data = objOrdersList;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            };
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetCurrentDayOrdersDetail()
        {
            var objOrdersList = await (from o in _context.Orders
                                       where o.CompanyId == _LoggedIn_CompanyId && o.DateCreated==DateTime.Now.Date
                                       let dealIds = _context.OrderDetail.Where(x => x.OrderId == o.Id && x.DealId != null).GroupBy(x => x.DealId).Select(g => new
                                       {
                                           Id = g.Max(row => row.Id)
                                       }).Select(x => x.Id.ToString()).ToList()
                                       select new GetOrderByIdDto
                                       {
                                           Id = o.Id,
                                           UserId = o.UserId,
                                           UserName = o.ObjUser.FullName,
                                           UserImage= _configuration.GetSection("AppSettings:SiteUrl").Value + o.ObjUser.FilePath + '/' + o.ObjUser.FileName,
                                           ContactNumber = o.ObjUser.ContactNumber,
                                           CompanyId = o.CompanyId,
                                           CompanyName = o.ObjCompany.Name,
                                           OrderStatus = ((Helpers.Enums.OrderStatus)o.OrderStatus).ToString(),
                                           PaymentMethodType = o.PaymentMethodType,
                                           DeliveryAddress = o.ObjUser.Address,
                                           DeliveryCharges = o.DeliveryCharges,
                                           ObjGetAllOrderDetail = (from odtl in _context.OrderDetail
                                                                   where (odtl.OrderId == o.Id && odtl.DealId == null)
                                                                   || (dealIds.Contains(odtl.Id.ToString())
                                                                   )
                                                                   select new GetAllOrderDetailDto
                                                                   {
                                                                       Id = odtl.Id,
                                                                       DealId = odtl.DealId,
                                                                       BillGroup = odtl.BillGroup,
                                                                       objDeals = (from odldeal in _context.OrderDetail
                                                                                   where odldeal.OrderId == o.Id && (odldeal.DealId == (odtl.DealId == null ? 0 : odtl.DealId)) && odldeal.BillGroup == odtl.BillGroup
                                                                                   select new DealDto
                                                                                   {
                                                                                       OrderDetailId = odldeal.Id,
                                                                                       dealId = odldeal.DealId,
                                                                                       billGroup = odldeal.BillGroup,
                                                                                       itemId = odldeal.ItemId,
                                                                                       Name = odldeal.ObjItem.Name,
                                                                                       FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + odldeal.ObjDeal.FilePath + '/' + odldeal.ObjDeal.FileName
                                                                                   }).ToList(),
                                                                       DealName = odtl.ObjDeal.Title,
                                                                       DealPrice = odtl.ObjDeal.Price,
                                                                       ItemId = odtl.ItemId,
                                                                       ItemName = odtl.DealId != null ? string.Empty : odtl.ObjItem.Name,
                                                                       ItemSizeId = odtl.ItemSizeId,
                                                                       ItemSizeName = odtl.ObjItemSize.SizeDescription,
                                                                       FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + odtl.ObjItem.FilePath + '/' + odtl.ObjItem.FileName,
                                                                       ItemSizePrice = odtl.ObjItemSize.Price > 0 ? odtl.ObjItemSize.Price : odtl.ObjItem.Price,
                                                                       CrustId = odtl.ObjCrust.Id,
                                                                       CrustName = odtl.ObjCrust.Name,
                                                                       CrustPrice = odtl.ObjCrust.Price,
                                                                       ObjAdditionalDetails = (from t in _context.Toppings
                                                                                               join idtl in _context.OrderDetailAdditionalDetails
                                                                                               on new { x1 = t.Id, x2 = (int)Helpers.Enums.ReferenceSection.Toppings, x3 = odtl.Id }
                                                                                               equals new { x1 = idtl.ReferenceId, x2 = idtl.ReferenceTypeId, x3 = idtl.OrderDetailId }
                                                                                               into newidtl
                                                                                               from idtl in newidtl.DefaultIfEmpty()
                                                                                               where odtl.ItemSizeId != null ? t.ItemId == odtl.ItemId && t.ItemSizeId == odtl.ItemSizeId : false

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
                                                                       Quantity = odtl.Quantity,
                                                                       OrderType = ((Helpers.Enums.OrderType)o.OrderType).ToString(),
                                                                       //SubTotal=odtl.DealId==null?(odtl.ObjItemSize.Price+odtl.Quantity)+(ObjAdditionalDetails.)
                                                                   }).ToList(),
                                           CreatedById = o.CretedById,
                                           DateCreated = o.DateCreated,
                                           UpdatedById = o.UpdateById,
                                           DateModified = o.DateModified,
                                           //TotalAmount = SubTotal.Select(x=> x.Price * x.Quantity).Sum()

                                       }).ToListAsync();

            foreach (var ord in objOrdersList)
            {
                foreach (var item in ord.ObjGetAllOrderDetail)
                {
                    item.SubTotal = item.DealId == null ? (item.ItemSizePrice.Value * item.Quantity) + ((item.ObjAdditionalDetails.Where(x => x.IsSelected == true).Sum(x => x.Price)) * item.Quantity) :
                               (item.DealDiscountPrice != null && item.DealDiscountPrice != 0) ? (item.DealDiscountPrice.Value * item.Quantity)
                               : (item.DealPrice.Value * item.Quantity);
                }

                ord.TotalAmount = ord.ObjGetAllOrderDetail.Sum(x => x.SubTotal) + (ord.DeliveryCharges ?? 0);
            };

            if (objOrdersList.Count > 0)
            {
                _serviceResponse.Data = objOrdersList;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            };
            return _serviceResponse;
        }
        public int GetCalculatedAmountByOrderId(int orderId,int DeliveryCharges=0)
        {                  
            var objOrderdtls =(from odtl in _context.OrderDetail
                                where odtl.OrderId == orderId
                               group odtl by new
                                {
                                    Id = odtl.DealId == null ? odtl.Id : 0,
                                    odtl.OrderId,
                                    odtl.DealId,
                                    odtl.BillGroup,
                                    ItemId= odtl.BillGroup == 0?odtl.ItemId:0,
                                    ItemSizeId= odtl.ItemId != null && odtl.BillGroup == 0 ? odtl.ItemSizeId:0,
                                    ItemDescrition= odtl.ItemId != null && odtl.BillGroup == 0?odtl.ObjItem.Description:string.Empty,
                                    Price = odtl.ItemId != null && odtl.BillGroup == 0 ? odtl.ObjItemSize.Price > 0 ? odtl.ObjItemSize.Price : odtl.ObjItem.Price :0,
                                    DealPrice= odtl.DealId != null?odtl.ObjDeal.Price : 0,
                                    DealDiscountPrice=odtl.DealId != null && odtl.ObjDeal.DiscountAmount!=null?odtl.ObjDeal.DiscountAmount : 0,
                                    odtl.Quantity,
                                   //SubTotal=odtl.SubTotal!=0?odtl.SubTotal:0

                               } into gcs
                                select new
                                {
                                    id = gcs.Key.Id,
                                    orderId = gcs.Key.OrderId,
                                    dealId = gcs.Key.DealId,
                                    billGroup = gcs.Key.BillGroup,
                                    itemId = gcs.Key.ItemId,
                                    itemSizeId= gcs.Key.ItemSizeId,
                                    price= gcs.Key.Price,
                                    dealPrice= gcs.Key.DealPrice,
                                    quantity= gcs.Key.Quantity,
                                    discountAmount = gcs.Key.DealDiscountPrice,
                                    ItemDescrition=gcs.Key.ItemDescrition
                                    ////subTotal= gcs.Key.SubTotal
                                }).ToList();

            var record = new GetAllOrderDto
            {
                Id = orderId,
                ObjGetAllOrderDetail =(from n in objOrderdtls
                                             let odtlData = from idtl in _context.OrderDetail
                                                       where n.dealId == null && idtl.Id == n.id
                                                       select idtl                                             
                                        select new GetAllOrderDetailDto
                                        {
                                            Id = n.id,
                                            OrderId = orderId,
                                            DealId = n.dealId,
                                            DealPrice = n.dealPrice,
                                            ItemId = n.itemId,
                                            ItemDescription = n.ItemDescrition,
                                            ItemSizeId =n.itemSizeId,
                                            ItemSizePrice =n.price,
                                            ObjAdditionalDetails = (from t in _context.Toppings
                                                                    join idtl in _context.OrderDetailAdditionalDetails
                                                                    on new { x1 = t.Id, x2 = (int)Helpers.Enums.ReferenceSection.Toppings, x3 = n.id } equals new { x1 = idtl.ReferenceId, x2 = idtl.ReferenceTypeId, x3 = idtl.OrderDetailId }
                                                                    into newidtl
                                                                    from idtl in newidtl.DefaultIfEmpty()
                                                                    where t.ItemId == n.itemId
                                                                            && t.ItemSizeId == n.itemSizeId

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

                                            Quantity =n.quantity,
                                            DealDiscountPrice=n.discountAmount                                         
                                        }).ToList()
                        
            };
            foreach (var item in record.ObjGetAllOrderDetail)
            {                
                 item.SubTotal = item.DealId == null?(item.ItemSizePrice.Value * item.Quantity) + ((item.ObjAdditionalDetails.Where(x => x.IsSelected == true).Sum(x => x.Price)) * item.Quantity):
                                (item.DealDiscountPrice != null && item.DealDiscountPrice != 0) ? (item.DealDiscountPrice.Value * item.Quantity)
                                : (item.DealPrice.Value * item.Quantity);               
            };

            record.TotalAmount = record.ObjGetAllOrderDetail.Sum(x => x.SubTotal)+DeliveryCharges;           

            return  record.TotalAmount;
        }
    }
}
