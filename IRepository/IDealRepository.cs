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
        Task<ServiceResponse<object>> AddDealData(AddDealDataDto dtoData);
        Task<ServiceResponse<object>> EditDealData(int id, EditDealDataDto dtoData);
        Task<ServiceResponse<object>> DeleteDealById(int id);
        Task<ServiceResponse<object>> GetNewDealDetailsById(int id);
    }
}
