﻿using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface ICategoryRepository
    {
        Task<ServiceResponse<object>> AddCategory(AddCategoryDto dtoData);
        Task<ServiceResponse<object>> EditCategory(int id, EditCategoryDto dtoData);
        Task<ServiceResponse<object>> GetAllCategories(int? companyId);
        Task<ServiceResponse<object>> GetCategoryById(int id);
    }
}