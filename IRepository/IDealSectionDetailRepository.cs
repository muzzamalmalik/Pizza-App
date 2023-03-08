using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IDealSectionDetailRepository
    {
        Task<ServiceResponse<object>> AddDealSectionDetail(AddDealSectionDetailDto dtoData);
        Task<ServiceResponse<object>> EditDealSectionDetail(int id, EditDealSectionDetailDto dtoData);
        //Task<ServiceResponse<object>> GetAllDealSectionDetail();
    }
}
