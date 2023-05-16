
using Microsoft.AspNetCore.Http;
using PizzaOrder.Context;
using PizzaOrder.Helpers;
using System;
using System.Security.Claims;
namespace PizzaOrder.Repository
{
    public class BaseRepository
    {
        protected int _LoggedIn_UserID = 0;
        protected int _LoggedIn_UserTypeId = 0;
        protected string _LoggedIn_UserName = "";
        protected string _LoggedIn_UserRole = "";
        protected int _LoggedIn_CompanyId = 0;

        protected ServiceResponse<object> _serviceResponse;
        protected DataContext _context;
        public BaseRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enums.ClaimType.UserId.ToString()));
            _LoggedIn_UserTypeId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enums.ClaimType.UserTypeId.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enums.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_CompanyId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enums.ClaimType.CompanyId.ToString()));
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(Enums.ClaimType.Role.ToString());

            _serviceResponse = new ServiceResponse<object>();
            _context = context;
            //if (_LoggedIn_UserID == 0)
            //    throw new Exception(CustomMessage.UserNotLoggedIn);
        }

    }
}
