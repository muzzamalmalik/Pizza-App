using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PizzaOrder.Context;
using PizzaOrder.IRepository;
using PizzaOrder.Repository;

namespace PizzaOrder.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ISlideShowRepository, SlideShowRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemSizeRepository, ItemSizeRepository>();
            services.AddScoped<ICrustRepository, CrustRepository>();
            services.AddScoped<IToppingRepository, ToppingRepository>();
            services.AddScoped<IDealRepository, DealRepository>();
            services.AddScoped<IDealSectionRepository, DealSectionRepository>();
            services.AddScoped<IDealSectionDetailRepository, DealSectionDetailRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IBillPaymentsRepository, BillPaymentsRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            


            services.AddDbContext<DataContext>(x => x.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            return services;
        }
    }
}
