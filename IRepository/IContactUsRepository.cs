using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IContactUsRepository
    {
        Task<ServiceResponse<object>> AddContactPersonInfo(AddContactUsDto dtoData);
        Task<ServiceResponse<object>> GetContactPersonInfoList(int CompanyId);
    }
}
