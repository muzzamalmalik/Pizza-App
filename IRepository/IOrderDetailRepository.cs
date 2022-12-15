using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IOrderDetailRepository
    {
        Task<ServiceResponse<object>> AddOrderDetail(AddOrderDetailDto dtoData);
        Task<ServiceResponse<object>> EditOrderDetail(int id, EditOrderDetailDto dtoData);
        Task<ServiceResponse<object>> GetAllOrderDetail(int CompanyId);
        Task<ServiceResponse<object>> GetOrderDetailById(int id);
        Task<ServiceResponse<object>> AddToCartCall(AddToCartCallDto dtoData);
        Task<ServiceResponse<object>> DeleteOrderDetailById(int id);
    }
}
