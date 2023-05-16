using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IItemRepository
    {
        Task<ServiceResponse<object>> AddItem(AddItemDto dtoData);
        Task<ServiceResponse<object>> GetAllItem(int Categoryid, int page, int pageSize);
        Task<ServiceResponse<object>> EditItem(int id, EditItemDto dtoData);
        Task<ServiceResponse<object>> GetItemDetailsById(int id);
        Task<ServiceResponse<object>> GetAllItemByWord(GetItemsBySearchFields dtoData);
        Task<ServiceResponse<object>> GetAllItems(int CompanyId);
        Task<ServiceResponse<object>> GetItemById(int id);
        Task<ServiceResponse<object>> GetItemSearchbylocation(ItemSearchbylocationDto dtoData);
        Task<ServiceResponse<object>> GetItemsByCategoryandPrice(ItemsByCategoryandPriceDto dto);
        Task<ServiceResponse<object>> DeleteItemById(int id);
        //Task<ServiceResponse<object>> GetAllItemByPriceRange(int? min, int? max);
    }
}
