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
        public int? CategoryId { get; set; }
        public int? CompanyId { get; set; }
    }

}
