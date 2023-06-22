using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using BLL.Repositories;

namespace ApiService.Middleware
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate next;
        protected readonly ILogger _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this.next = next;
            _logger= logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("LogErrorHandlerMiddleware.Invoke");
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"something went wrong {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize("Internal server error"));
            }
        }
    }
}
