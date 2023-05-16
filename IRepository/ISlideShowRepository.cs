using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface ISlideShowRepository
    {
        Task<ServiceResponse<object>> AddSlideShow(SlideShowAddDto dtoData);
        Task<ServiceResponse<object>> GetAllSlideShows(int CompanyId);
        Task<ServiceResponse<object>> GetSliderDetailById(int ImageId);
        Task<ServiceResponse<object>> EditSlideShow(int ImageId, SlideShowEditDto dtoData);
        Task<ServiceResponse<object>> AddFeaturedAds(AddFeaturedAdsDto dtoData);
        Task<ServiceResponse<object>> GetAllFeaturedAds(double Lat, double Long, int Range);
        Task<ServiceResponse<object>> EditFeaturedAds(int id, EditFeaturedAdsDto dtoData);
        Task<ServiceResponse<object>> DeleteSliderById(int id);

    }
}
