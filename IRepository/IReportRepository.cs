using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IReportRepository
    {
        Task<ServiceResponse<object>> GetOrderListForReport();
        Task<ServiceResponse<object>> GetCustomerWiseOrderReport();
    }
}
