using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class DealSectionDto
    {
    }
    public class AddDealSectionDto
    {
        public int DealId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
        public bool IsActive { get; set; }
    }

    public class EditDealSectionDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
        public int Mode { get; set; }
        public bool IsActive { get; set; }

    }

    public class GetAllDealSectionDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; } = 1;
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        
        public bool MultiSelect { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    

    public class GetDealSectionByIdDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }
    }
}
