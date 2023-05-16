using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using System.Threading.Tasks;

namespace PizzaOrder.IRepository
{
    public interface IUserRepository
    {
        Task<ServiceResponse<object>> AddUser(AddUserDto dtoData);
        Task<ServiceResponse<object>> EditUser(int Id, EditUserDto dtoData);
        Task<ServiceResponse<object>> GetUserDetailById(int Id);
        Task<ServiceResponse<object>> GetAllUsers();
        Task<ServiceResponse<object>> GetUserRoleList();
        Task<ServiceResponse<object>> DeleteUser(int Id);
    }
}
