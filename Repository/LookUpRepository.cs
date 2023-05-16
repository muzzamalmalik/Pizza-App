using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PizzaOrder.Context;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PizzaOrder.Repository
{
    public class LookUpRepository : BaseRepository, ILookUpRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public LookUpRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }
        public async Task<ServiceResponse<object>> GetCategoriesList()
        {
            var list1 =await (from ca in _context.Category
                              where ca.IsActive==true
                         orderby ca.Id descending
                         select ca).ToListAsync();

            if (list1.Count() > 0)
            {
                _serviceResponse.Data = list1;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetItemsByCategoryId(int id=0)
        {
            if(id>0)
            {
                var list1 = await (from it in _context.Items
                                   where it.CategoryId==id && it.IsActive==true
                                   orderby it.Id descending
                                   select it).ToListAsync();
                if (list1.Count() > 0)
                {
                    _serviceResponse.Data = list1;
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = "Record Found";
                }
            }

            
            return _serviceResponse;
        }
    }
}
