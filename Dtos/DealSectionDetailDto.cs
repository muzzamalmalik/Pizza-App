using System;
using System.Data;

namespace PizzaOrder.Dtos
{
    public class DealSectionDetailDto
    {
    }

    public class AddDealSectionDetailDto
    {
       
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
    }

    public class EditDealSectionDetailDto
    {
        
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
    }

    public class GetAllDealSectionDetailDto
    {
        public int Id { get; set; }
        
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class GetDealSectionDetailByIdDto
    {
        public int Id { get; set; }
        
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
    }
}
