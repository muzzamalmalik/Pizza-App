using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PizzaOrder.Helpers;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace PizzaOrder.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException && ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    var response = new ServiceResponse<object>()
                    {
                        Success = false,
                        Message = CustomMessage.SqlDuplicateRecord,
                    };

                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    var json = JsonSerializer.Serialize(response, options);

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Environments.Development;
                    if (isDevelopment)
                    {
                        var response = new ServiceResponse<object>()
                        {
                            Success = false,
                            Message = (ex.Message + Environment.NewLine + (ex.InnerException is not null ? ex.InnerException.ToString() : "")) ?? ex.InnerException.ToString(),
                        };

                        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                        var json = JsonSerializer.Serialize(response, options);

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        await context.Response.WriteAsync(json);
                    }
                    else
                    {
                        if (ex.Message == CustomMessage.UserNotLoggedIn)
                        {
                            var response = new ServiceResponse<object>()
                            {
                                Success = false,
                                Message = (ex.Message + Environment.NewLine + (ex.InnerException is not null ? ex.InnerException.ToString() : "")) ?? ex.InnerException.ToString(),
                            };

                            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                            var json = JsonSerializer.Serialize(response, options);

                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            await context.Response.WriteAsync(json);
                        }
                        else
                        {
                            _logger.LogError(ex, ex.Message);
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                            var Message = "Method Name: " + new StackTrace(ex).GetFrame(0).GetMethod().Name + " | Message: " +
                                ex.Message + Environment.NewLine + (ex.InnerException is not null ? ex.InnerException.ToString() : "") ?? ex.InnerException.ToString();
                            var response = new ApiException(context.Response.StatusCode, Message, ex.StackTrace?.ToString());

                            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                            var json = JsonSerializer.Serialize(response, options);
                            await context.Response.WriteAsync(json);
                        }
                    }
                }
            }
        }
        public class ApiException
        {
            public ApiException(int statusCode, string message = null, string details = null)
            {
                StatusCode = statusCode;
                Message = message;
                Details = details;
            }
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string Details { get; set; }
        }
    }
}
