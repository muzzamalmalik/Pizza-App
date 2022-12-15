using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface ICrustRepository
    {
        Task<ServiceResponse<object>> AddCrust(AddCrustDto dtoData);
        Task<ServiceResponse<object>> EditCrust(int id, EditCrustDto dtoData);
        Task<ServiceResponse<object>> GetAllCrust(int CompanyId);
    }
}
