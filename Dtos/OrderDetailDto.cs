using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    //public class OrderDetailDto
    //{
    //}

    public class AddOrderDetailDto
    {
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public int? ItemSizeId { get; set; }
        public int? CrustId { get; set; }
        public int? ToppingId { get; set; }
        public int Quantity { get; set; }
        public int CompanyId { get; set; }
        public int OrderType { get; set; }
        public string Instructions { get; set; }
    }

    public class AddToCartCallDto
    {
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public int? ItemSizeId { get; set; }
        public int? CrustId { get; set; }
        public int Quantity { get; set; }
        public int OrderType { get; set; }
        public string Instructions { get; set; }
        public int SubTotal { get; set; }
        public List<AddToCartCallToppingssDto> objtopping { get; set; } = new();
    }
    public class AddToCartCallToppingssDto
    {
        public int? ItemSizeId { get; set; }
        public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        
    }
    public class EditOrderDetailDto
    {
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public int? ItemSizeId { get; set; }
        public int? CrustId { get; set; }
        public int? ToppingId { get; set; }
        public int Quantity { get; set; }
        public int CompanyId { get; set; }
        public int OrderType { get; set; }
        public string Instructions { get; set; }
        public int SubTotal { get; set; }
    }

    public class GetAllOrderDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        public string DealName { get; set; }
        public int DealPrice { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? ItemSizeId { get; set; }
        public string ItemSizeName { get; set; }
        public int ItemSizePrice { get; set; }
        public int? CrustId { get; set; }
        public string CrustName { get; set; }
        public int CrustPrice { get; set; }
        public List<GetAllToppingDto> ObjGetAllTopping { get; set; }
        public int Quantity { get; set; }
        public int CompanyId { get; set; }
        public string OrderType { get; set; }
        public string Instructions { get; set; }
        public int SubTotal { get; set; }
    }

    public class GetOrderDetailByIdDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        public string DealName { get; set; }
        public int DealPrice { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? ItemSizeId { get; set; }
        public string ItemSizeName { get; set; }
        public int ItemSizePrice { get; set; }
        public int? CrustId { get; set; }
        public string CrustName { get; set; }
        public int CrustPrice { get; set; }
        public int? ToppingId { get; set; }
        public string ToppingName { get; set; }
        public int ToppingPrice { get; set; }
        public int Quantity { get; set; }
        public int CompanyId { get; set; }
        public string OrderType { get; set; }
        public string Instructions { get; set; }
        public int SubTotal { get; set; }
    }

    
}
