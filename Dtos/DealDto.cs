using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class DealDto
    {
    }

    public class AddDealDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Percentage { get; set; }
        public int DiscountAmount { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public IFormFile ImageData { get; set; }
    }

    public class EditDealDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Percentage { get; set; }
        public int DiscountAmount { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public IFormFile ImageData { get; set; }
    }

    public class GetDealDetailsByIdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Percentage { get; set; }
        public int DiscountAmount { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public int DealId { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
        public List<GetAllFlavoursDto> ObjGetAllFlavours { get; set; }
        //public List<GetAllDealSectionDto> ObjGetAllDealSection { get; set; }
        //public List<GetAllDealSectionDetailDto> ObjGetAllDealSectionDetail { get; set; }
    }

    public class GetAllFlavoursDto
    {
        public int Id { get; set; }
        public string FlavourName { get; set; }
        public int CategoryId { get; set; }
    }

    public class GetAllDealDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Percentage { get; set; }
        public int DiscountAmount { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public List<GetAllDealSectionDto> ObjGetAllDealSection { get; set; }
        public List<GetAllDealSectionDetailDto> ObjGetAllDealSectionDetail { get; set; }
    }
}
