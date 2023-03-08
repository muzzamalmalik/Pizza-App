using Microsoft.AspNetCore.Mvc;
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
        //Task<ServiceResponse<object>> GetAllOrder(int OrderStatus);
        Task<ServiceResponse<object>> GetOrderById(int id, int orderStatus, int page, int pageSize);
        Task<ServiceResponse<object>> GetPaymentType();
        Task<ServiceResponse<object>> ProcessOrder(AddProcessOrderDto dtoData);
        Task<ServiceResponse<object>> GetCartList();
        Task<ServiceResponse<object>> GetOrderForRider(int CompanyId);
        Task<ServiceResponse<object>> RiderStuatusUpdate(AddProcessOrderDto dtoData);
        Task<ServiceResponse<object>> GetOrderByCompany(int CompanyId, int orderStatus, int page, int pageSize);
    }
}
