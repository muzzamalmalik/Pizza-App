using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IDealSectionRepository
    {
        Task<ServiceResponse<object>> AddDealSection(AddDealSectionDto dtoData);
        Task<ServiceResponse<object>> EditDealSection(int id, EditDealSectionDto dtoData);
        Task<ServiceResponse<object>> DeleteDealSectionById(int id);
        //Task<ServiceResponse<object>> GetAllDealSection(int CompanyId);
    }
}
