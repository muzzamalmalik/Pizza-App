using Microsoft.AspNetCore.Http;
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
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int CategoryId { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public IFormFile ImageData { get; set; }
    }
    public class EditItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Sku { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int CategoryId { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        public IFormFile ImageData { get; set; }

    }
    public class GetAllItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int Price { get; set; }
        public int Sku { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public int CategoryId { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        
    }
    public class GetItemDetailsByIdDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string FullPath { get; set; }
        public List<GetAllItemSizeDto> objGetAllItemSize { get; set; }
        public List<GetAllCrustDto> objGetAllCrust { get; set; }
        public List<GetAllToppingDto> objGetAllTopping { get; set; }
    }
}
