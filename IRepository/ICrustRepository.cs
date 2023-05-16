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
        Task<ServiceResponse<object>> GetAllCrust();
        Task<ServiceResponse<object>> AddNewCrust(AddNewCrustDto dtoData);
        Task<ServiceResponse<object>> EditNewCrust(int id, EditNewCrustDto dtoData);
        Task<ServiceResponse<object>> GetAllNewCrust();
        Task<ServiceResponse<object>> DeleteCrustById(int id);
        Task<ServiceResponse<object>> GetAllCrustbyId(int Id);

    }
}
