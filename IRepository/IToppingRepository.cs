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
    }
}
