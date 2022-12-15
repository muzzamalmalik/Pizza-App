using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class ItemSizeDto
    {
    }

    public class AddItemSizeDto
    {
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditItemSizeDto
    {
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
    }
    public class GetAllItemSizeDto
    {
        public int Id { get; set; }
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
    }

    public class GetItemSizeByIdDto
    {
        public int Id { get; set; }
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
    }
}
