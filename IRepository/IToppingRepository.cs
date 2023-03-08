using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IToppingRepository
    {
        Task<ServiceResponse<object>> AddTopping(AddToppingDto dtoData);
        Task<ServiceResponse<object>> EditTopping(int id, EditToppingDto dtoData);
        Task<ServiceResponse<object>> GetAllTopping(int CompanyId);
        Task<ServiceResponse<object>> AddNewTopping(AddNewToppingDto dtoData);
        Task<ServiceResponse<object>> EditNewTopping(int id, EditNewToppingDto dtoData);
        Task<ServiceResponse<object>> GetAllNewTopping(int ComapnyId);
        Task<ServiceResponse<object>> GetAllToppingById(int Id);
    }
}
