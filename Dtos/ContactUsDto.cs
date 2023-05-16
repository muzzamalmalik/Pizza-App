namespace PizzaOrder.Dtos
{
    public class ContactUsDto
    {
    }
    public class AddContactUsDto
    {
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int CompanyId { get; set; }
    }  
     public class GetAllContactPersonInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int CompanyId { get; set; }
    }
}
