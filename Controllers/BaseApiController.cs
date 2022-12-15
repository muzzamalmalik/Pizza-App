
using Microsoft.AspNetCore.Mvc;
using PizzaOrder.Helpers;

namespace PizzaOrder.Controllers
{
   [Route("api/[controller]")]
    [ApiController]

    public class BaseApiController : ControllerBase
    {
        protected ServiceResponse<object> _response;
        public BaseApiController()
        {
            _response = new ServiceResponse<object>();

        }
    }
}
