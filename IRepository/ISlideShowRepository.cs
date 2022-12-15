using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface ISlideShowRepository
    {
        Task<ServiceResponse<object>> AddSlideShow(SlideShowAddDto dtoData);
        Task<ServiceResponse<object>> GetAllSlideShows(int CompanyId);
        Task<ServiceResponse<object>> EditSlideShow(int id, int imageid, SlideShowEditDto dtoData);

    }
}
