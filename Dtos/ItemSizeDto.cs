﻿using System;
using System.Collections.Generic;

namespace PizzaOrder.Dtos
{
    public class ItemSizeDto
    {
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddItemSizeDto
    {
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
    }

    public class EditItemSizeDto
    {
        public int Id { get; set; }
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }

    }
    public class GetAllItemSizeDto
    {
        public int Id { get; set; }
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class GetItemSizeByIdDto
    {
        public int Id { get; set; }
        public string SizeDescription { get; set; }
        public int Price { get; set; }
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
