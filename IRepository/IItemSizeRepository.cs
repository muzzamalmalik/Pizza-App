using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IItemSizeRepository
    {
        Task<ServiceResponse<Object>> AddItemSize(AddItemSizeDto dtoData);
        Task<ServiceResponse<object>> EditItemSize(int id, EditItemSizeDto dtoData);
        Task<ServiceResponse<object>> GetAllItemSize();
        Task<ServiceResponse<object>> GetItemSizeById(int id);
    }
}
