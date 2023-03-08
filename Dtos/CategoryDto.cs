using System;
using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class CategoryDto
    {
    }

    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
    }

    public class EditCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
    }


    public class GetAllCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int? ItemsCount { get; set; }
        public int CompanyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //public List<GetAllItemDto> objGetAllItem { get; set; }
    }
    public class GetCategoryByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ItemsCount { get; set; }
        public int CompanyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
