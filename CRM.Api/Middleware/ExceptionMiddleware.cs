using CRM.Exceptions;
using CRM.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException capturada");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.StatusCode;
                var payload = ResponseModel<object>.Erro(ex.Message);
                await context.Response.WriteAsJsonAsync(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var payload = ResponseModel<object>.Erro("Erro inesperado.");
                await context.Response.WriteAsJsonAsync(payload);
            }
        }
    }
}
