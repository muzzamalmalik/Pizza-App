using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface ICompanyRepository
    {
        Task<ServiceResponse<object>> AddCompany(AddCompanyDto dtoData);
        Task<ServiceResponse<object>> EditCompany(int id, EditCompanyDto dtoData);
        Task<ServiceResponse<object>> GetAllCompany();
        Task<ServiceResponse<object>> GetCompanyById(int id);
        Task<ServiceResponse<object>> GetAllCompanyByLatLong(int Range, double Lat, double Long);
        Task<ServiceResponse<object>> SearchCompany(string SearchField);

    }
}
