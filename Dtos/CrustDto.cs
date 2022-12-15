namespace PizzaOrder.Dtos
{
    public class CrustDto
    {
    }

    public class AddCrustDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public int ItemId { get; set; }
        public int ItemSizeId { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditCrustDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public int ItemId { get; set; }
        public int ItemSizeId { get; set; }
        public int CompanyId { get; set; }
    }

    public class GetAllCrustDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public int ItemId { get; set; }
        public int ItemSizeId { get; set; }
        public int CompanyId { get; set; }
    }
}
