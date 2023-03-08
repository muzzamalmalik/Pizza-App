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
       
}
