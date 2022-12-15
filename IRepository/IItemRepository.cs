using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IItemRepository
    {
        Task<ServiceResponse<object>> AddItem(AddItemDto dtoData);
        Task<ServiceResponse<object>> GetAllItem(int Categoryid);
        Task<ServiceResponse<object>> EditItem(int id, EditItemDto dtoData);
        Task<ServiceResponse<object>> GetItemDetailsById(int id);
    }
}
