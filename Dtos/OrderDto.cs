using System;
using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class OrderDto
    {
    }

    public class AddOrderDto
    {
        public int OrderStatus { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditOrderDto
    {
        public int OrderStatus { get; set; }
        public int DeliveryCharges { get; set; }
        public int Amount { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }

    public class GetAllOrderDto
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PaymentMethodType { get; set; }
        public string DeliveryAddress { get; set; }
        public string Instructions { get; set; }
        public int TotalAmount { get; set; }
        public int CompanyId { get; set; }
       // public string CompanyName { get; set; }
        public int? DeliveryCharges { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public List<GetAllOrderDetailDto> ObjGetAllOrderDetail { get; set; }
    }

    public class GetAllOrdersDto
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public int UserId { get; set; }
        public string PaymentMethodType { get; set; }
        //public string DeliveryAddress { get; set; }
        public int TotalAmount { get; set; }
        public int CompanyId { get; set; }
        public int? DeliveryCharges { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public List<GetAllOrderDetailDto> ObjGetAllOrderDetail { get; set; }
    }

    public class GetPaymentTypeDto
    {
        public string PaymentType { get; set; }
        public int Id { get; set; } 
        public string Name { get; set; }
        public string AccountNumber { get; set; }
    }

    public class AddProcessOrderDto
    {
        public int OrderId { get; set; }
        public int OrderStatus { get; set; }
        public string Instructions { get; set; }
        public int PaymentMethodType { get; set; }
        public string SecoundaryAddress { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
    public class GetOrderByIdDto
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public int UserId { get; set; }
        public string UserImage { get; set; }
        public string UserName { get; set; }
        public string ContactNumber { get; set; }
        public string PaymentMethodType { get; set; }
        public string DeliveryAddress { get; set; }
        public int TotalAmount { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? DeliveryCharges { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
      
        public List<GetAllOrderDetailDto> ObjGetAllOrderDetail { get; set; }
    }
    public class GetOrderForRiderDto
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ContactNumber { get; set; }
        public string PaymentMethodType { get; set; }
        public string DeliveryAddress { get; set; }
    } 
    public class GetOrderListForReportDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ItemName { get; set; }
        public string DealName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
        public string PaymentMethodType { get; set; }
        public string DeliveryAddress { get; set; }
        public string Rider { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }

    public class GetMasterOrderDetailListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string OrderNumebr { get; set; }
        public int Amount { get; set; }
        public int DeliveryCharges { get; set; }
        public string UpdatedBy { get; set; }
        public List<GetOrderDetailListDto> ObjOrderDetail { get; set; }
        public int ItemId { get; set; }
        public int DealId { get; set; }
        public int OrderType { get; set; }
        public int CompanyId { get; set; }

        public int BillGroup { get; set; }
        public int OderDetailId { get; set; }
    }
    public class GetOrderDetailListDto
    {
        public int? Id { get; set; }
        public int? Quantity { get; set; }
        public string OrderType { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemSizeId { get; set; }
        public string ItemSizeName { get; set; }
        public int ItemPrice { get; set; }
        public int DealId { get; set; }
        public string DealTitle { get; set; }
        public bool IsDeal { get; set; }
        public List<GetDealListDto> ObjDealDetail { get; set; }

    }

    public class GetDealListDto
    {       
        public List<GetItemListDto> ObjItemDetail { get; set; }
        public int DealId { get; set; }
        public int DealPrice { get; set; }
        public int Quantity { get; set; }

    }
    public class GetItemListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ItemSizeId { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }

    }
    public class GetAllOrdersList
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string OrderNumebr { get; set; }
        public int Amount { get; set; }
        public int CompanyId { get; set; }
        public int DeliveryCharges { get; set; }
        public string UpdatedBy { get; set; }
        public List<GetItemListDto> ObjItemsList { get; set; }
        public List<GetDealListDto> ObjDealsList { get; set; }
    }


}
