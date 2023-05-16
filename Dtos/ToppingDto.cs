using System;
using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class ToppingDto
    {
    }

    public class AddToppingDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int ItemSizeId { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditToppingDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int ItemSizeId { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
    }

    public class GetAllToppingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int? ItemId { get; set; }
        public int? ItemSizeId { get; set; }
        public string? ItemSizeName { get; set; }
        public int? CategoryId { get; set; }
        public int? CompanyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
    public class AddNewToppingDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public List<int> ItemId { get; set; }
        public int ItemSizeId { get; set; }
        // public int CategoryId { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditNewToppingDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public int ItemId { get; set; }
        public int ItemSizeId { get; set; }
        // public int CategoryId { get; set; }
        public int CompanyId { get; set; }
    }

    public class GetAllNewToppingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemName { get; set; }
        public string ItemSizeName { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public int? ItemId { get; set; }
        public int? ItemSizeId { get; set; }
        //public int? CategoryId { get; set; }
        public int? CompanyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
