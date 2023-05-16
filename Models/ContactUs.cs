namespace PizzaOrder.Models
{
    public class ContactUs : BaseEntity
    {
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int CompanyId { get; set; }
    }
}
