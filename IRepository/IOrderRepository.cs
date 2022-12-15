using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IOrderRepository
    {
        Task<ServiceResponse<object>> AddOrder(AddOrderDto dtoData);
        Task<ServiceResponse<object>> EditOrder(int id, EditOrderDto dtoData);
        Task<ServiceResponse<object>> GetAllOrder();
        Task<ServiceResponse<object>> GetPaymentType(int id);
        Task<ServiceResponse<object>> AddCheckOutDetails(AddCheckOutDetailsDto dtoData);
        Task<ServiceResponse<object>> GetCartList();
    }
}
