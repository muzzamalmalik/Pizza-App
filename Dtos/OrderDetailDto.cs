using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace PizzaOrder.Dtos
{
    //public class OrderDetailDto
    //{
    //}

    public class AddOrderDetailDto
    {
        public int OrderId { get; set; }
        public int DealId { get; set; }
        //public int CategoryId { get; set; }
        public int ItemId { get; set; }
        public int ItemSizeId { get; set; }
        public int CrustId { get; set; }
        public int ToppingId { get; set; }
        public int Quantity { get; set; }
        //public int CompanyId { get; set; }
        public int OrderType { get; set; }
        //public string Instructions { get; set; }
    }

    public class AddToCartCallDto
    {
        public int OrderId { get; set; }
        public int OrderType { get; set; }
        //public string Instructions { get; set; }
        public int CompanyId { get; set; }
        public int DeliveryCharges { get; set; }
        public int MethodType { get; set; }
        public List<OrderDetailDto> objOrderDetail{ get; set; }

    }


    public class DealsDto
    {
        public int itemId { get; set; }
        public int Quantity { get; set; }


    }
    public class GetDealsItemsListDto
    {
        public List<GetAllDealsItemDto> objItemList { get; set; }
        public int Quantity { get; set; }

    }
    public class GetAllDealsItemDto
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public string ItemName { get; set; }


    }
    public class OrderDetailAdditionalDetailsDto
    {
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int OrderDetailId { get; set; }
        public int ReferenceTypeId { get; set; }
        public bool IsSelected { get; set; }

    }

    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        //public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public int? ItemSizeId { get; set; }
        public int? CrustId { get; set; }
        //public int? ToppingId { get; set; }
        public int Quantity { get; set; }
        //public int CompanyId { get; set; }
        public int OrderType { get; set; }
        //public string Instructions { get; set; }
        public int SubTotal { get; set; }
        public bool IsDeal { get; set; }
        public List<AdditionalDetailsDto> objAdditionalDetails { get; set; }
        public List<OrderDetailDealDto> objDeals { get; set; }
    }

    public class AdditionalDetailsDto
    {
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public int OrderDetailId { get; set; }
        public int ReferenceTypeId { get; set; }
    }

    public class OrderDetailDealDto
    {
        public int OrderId { get; set; }
        public int DealId { get; set; }
        public int ItemId { get; set; }
        //public int Quantity { get; set; }
        public int OrderType { get; set; }
        //public int SubTotal { get; set; }
        public int BillGroup { get; set; }
    }

    public class GetAllOrderDetailDto
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        public string DealName { get; set; }
        public string ItemDescription { get; set; }
        public int? DealPrice { get; set; }
        //public int? CategoryId { get; set; }
        //public string CategoryName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? ItemSizeId { get; set; }
        public string ItemSizeName { get; set; }
        public string FullPath { get; set; }
        public int? ItemSizePrice { get; set; }
        public int? CrustId { get; set; }
        public string CrustName { get; set; }
        public int? CrustPrice { get; set; }
        public int? DealDiscountPrice { get; set; }
        public List<OrderDetailAdditionalDetailsDto> ObjAdditionalDetails { get; set; }
        public List<DealDto> objDeals { get; set; }
        public int Quantity { get; set; }
        //public int CompanyId { get; set; }
        public string OrderType { get; set; }
        //public string Instructions { get; set; }
        public int SubTotal { get; set; }
        public int BillGroup { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class GetOrderDetailByIdDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        public string DealName { get; set; }
        public int DealPrice { get; set; }
        //public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? ItemSizeId { get; set; }
        public string ItemSizeName { get; set; }
        public int ItemSizePrice { get; set; }
        public int? CrustId { get; set; }
        public string CrustName { get; set; }
        public int CrustPrice { get; set; }
        //public int? ToppingId { get; set; }
        //public string ToppingName { get; set; }
        //public int ToppingPrice { get; set; }
        public int Quantity { get; set; }
        //public int CompanyId { get; set; }
        public string OrderType { get; set; }
        //public string Instructions { get; set; }
        public int SubTotal { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    
}
