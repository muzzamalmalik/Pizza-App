using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class ItemDto
    {
    }

    public class AddItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sku { get; set; }
        public int? Price { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int CategoryId { get; set; }
        public bool ActiveQueue { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public IFormFile ImageData { get; set; }
        //public List<ItemSizeDto> ObjItemSize { get; set; }
        public string ObjItemSizeStr { get; set; }

    }
    public class EditItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Sku { get; set; }
        public int ItemSizeId { get; set; }
        public int Price { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int CategoryId { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public IFormFile ImageData { get; set; }
        public string ObjItemSizeStr { get; set; }


    }
    public class GetAllItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int Price { get; set; }
        public int Sku { get; set; }
        public string ItemSize { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        //public bool ActiveQueue { get; set; }
        //public int CompanyId { get; set; }
        //public int CreatedById { get; set; }
        //public int? UpdatedById { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }

    }

    public class GetItemsBySearchFields
    {
        public string SearchField { get; set; }
        public string CategoryId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool SearchFrom { get; set;}
        public int CompanyId { get; set;}
    }

    public class GetItemDetailsByIdDto
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FullPath { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedById { get; set; }    
        public int Sku { get; set; }
        public string CategoryName { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public List<GetAllItemSizeDto> objGetAllItemSize { get; set; }
        public List<GetAllCrustDto> objGetAllCrust { get; set; }
        public List<GetAllToppingDto> objGetAllTopping { get; set; }
    }

    public class GetItemByIdDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string FullPath { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //public List<GetAllItemSizeDto> objGetAllItemSize { get; set; }
        //public List<GetAllCrustDto> objGetAllCrust { get; set; }
        //public List<GetAllToppingDto> objGetAllTopping { get; set; }
    }
    public class ItemSearchbylocationDto
    {
        public string SearchField { get; set; }
        public string CategoryId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool SearchFrom { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int Range { get; set; }
    
    }
    public class ItemsByCategoryandPriceDto
    {
        public List<int> Id { get; set; }
        public int? MinPrice{ get; set; }
        public int? MaxPrice { get; set; }

    }
}
