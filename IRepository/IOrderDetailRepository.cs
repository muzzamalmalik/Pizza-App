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
        Task<ServiceResponse<object>> EditOrderDetail(int id, OrderDetailDto dtoData);
        Task<ServiceResponse<object>> GetAllOrderDetail();
        Task<ServiceResponse<object>> GetOrderDetailById(int id);
        Task<ServiceResponse<object>> AddToCartCall(AddToCartCallDto dtoData);
        Task<ServiceResponse<object>> DeleteOrderDetailById(string id);
        Task<ServiceResponse<object>> DeleteOrderDetailByOrderDetailId(int id, int? dealId);
        Task<ServiceResponse<object>> GetDealItemsListById(int id, int CategoryId = 0);
    }
}
