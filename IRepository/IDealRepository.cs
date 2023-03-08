using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IDealRepository
    {
        Task<ServiceResponse<object>> AddDeal(AddDealDto dtoData);
        Task<ServiceResponse<object>> EditDeal(int id, EditDealDto dtoData);
        Task<ServiceResponse<object>> GetAllDeal(int CompanyId, int page, int pageSize);
        Task<ServiceResponse<object>> GetDealDetailsById(int id);
    }
}
