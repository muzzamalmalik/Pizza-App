using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface ILookUpRepository
    {
        Task<ServiceResponse<object>> GetCategoriesList();
        Task<ServiceResponse<object>> GetItemsByCategoryId(int id = 0);
    }
}
