using System.Data;

namespace PizzaOrder.Dtos
{
    public class DealSectionDetailDto
    {
    }

    public class AddDealSectionDetailDto
    {
        public int DealId { get; set; }
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditDealSectionDetailDto
    {
        public int DealId { get; set; }
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
    }

    public class GetAllDealSectionDetailDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
        public string ItemName { get; set; }
    }

    public class GetDealSectionDetailByIdDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
    }
}
