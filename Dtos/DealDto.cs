using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace PizzaOrder.Dtos
{
    public class DealDto
    {
        public int OrderDetailId { get; set; }
        public int? dealId { get; set; }
        public int? itemId { get; set; }
        public int billGroup { get; set; }
        public string Name { get; set; }
    }

    public class AddDealDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? Percentage { get; set; }
        public int? DiscountAmount { get; set; }
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
        public int? Percentage { get; set; }
        public int? DiscountAmount { get; set; }
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
        public int? Percentage { get; set; }
        public int? DiscountAmount { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public List<GetDealSectionDto> ObjGetDealSection { get; set; }
        //public List<GetAllDealSectionDto> ObjGetAllDealSection { get; set; }
        //public List<GetAllDealSectionDetailDto> ObjGetAllDealSectionDetail { get; set; }
    }

    public class GetDealSectionDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
        public List<GetAllFlavoursDto> ObjGetAllFlavours { get; set; }
    }

    public class GetAllFlavoursDto
    {
        public int Id { get; set; }
        public string FlavourName { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class GetAllDealDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? Percentage { get; set; }
        public int? DiscountAmount { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public List<GetAllDealSectionDto> ObjGetAllDealSection { get; set; }
        public List<GetAllDealSectionDetailDto> ObjGetAllDealSectionDetail { get; set; }
    }
}
