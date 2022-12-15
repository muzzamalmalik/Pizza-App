using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IBillPaymentsRepository
    {
        Task<ServiceResponse<object>> AddBillPayments(AddBillPaymentsDto dtoData);
        Task<ServiceResponse<object>> GetAllBillPayments(int CompanyId);
        Task<ServiceResponse<object>> GetBillPaymentsById(int id);
    }
}
