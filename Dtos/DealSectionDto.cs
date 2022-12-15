using AutoMapper.Configuration.Conventions;
using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class DealSectionDto
    {
    }

    public class AddDealSectionDto
    {
        public int DealId { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditDealSectionDto
    {
        public int DealId { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
        public int CompanyId { get; set; }
    }

    public class GetAllDealSectionDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
        public int CompanyId { get; set; }
        public string CategoryName { get; set; }
    }

    

    public class GetDealSectionByIdDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
        public int CompanyId { get; set; }
    }
}
