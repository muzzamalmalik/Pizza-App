
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
   public interface IAuthRepository
    {
        Task<ServiceResponse<object>> Register(UserForRegisterDto model);
        Task<ServiceResponse<object>> VerifyUser(VerifyUserDto model);
        Task<ServiceResponse<object>> Login(UserForLoginDto model);
        Task<ServiceResponse<object>> Logout();
        Task<ServiceResponse<object>> GetProfileData();
        Task<ServiceResponse<object>> GetAllUsers();
        Task<ServiceResponse<object>> GetAdminlogoAndBrandLogoData(int CompanyId);
        Task<bool> UserExists(string userName);

        Task<ServiceResponse<object>> EditUser(int id, UserForEditDto model);
        Task<ServiceResponse<object>> EditUserImagebyApp(int id, UserForEditDtoAdd model);
    }
}

