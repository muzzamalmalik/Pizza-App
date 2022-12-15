
using AutoMapper;
//using FabTimes.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PizzaOrder.Extensions;
using PizzaOrder.Helpers;
using PizzaOrder.Hubs;

namespace PizzaOrder
{
    public class Startup
    {

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _HostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment HostEnvironment)
        {
            _config = configuration;
            _HostEnvironment = HostEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddApplicationServices(_config);
            services.AddCors();
            services.AddControllers(o =>
            {
                o.Conventions.Add(new ControllerNameAttributeConvention());
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddIdentityServices(_config);
            services.AddSignalR();

            SetService(services);
        }

        private static void SetService(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var accessor = serviceProvider.GetService<IHttpContextAccessor>();
            HelperFunctions.SetHttpContextAccessor(accessor);
        }
       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fabcot v1"));
            }

            //app.UseFileServer(new FileServerOptions{
            //   FileProvider = new  PhysicalFileProvider (
            //       Path.Combine(Directory.GetCurrentDirectory(), "StaticFile")), RequestPath = "/StaticFile"
            //});



            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
           
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "areas",
                //    pattern: "{area:exists}/{controller}/{action}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
                endpoints.MapHub<ChatHub>("/chart");
                //endpoints.MapHub<WebRtcHub>("/rtcHub");
                //endpoints.MapControllers();
            });
        }
    }
}
