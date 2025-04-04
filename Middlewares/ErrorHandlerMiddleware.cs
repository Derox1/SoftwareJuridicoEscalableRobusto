using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Mvc;
namespace API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;


        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error no controlado");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var error = new ProblemDetails
                {
                    Status = context.Response.StatusCode,
                    Title = "Error interno del servidor",
                    Detail = "Ocurrió un error inesperado. Contacta al soporte si persiste.",
                    Instance = context.Request.Path
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var json = JsonSerializer.Serialize(error, options);

                // ✅ Asegúrate de usar await aquí (sin return)
                await context.Response.WriteAsync(json);
            }


        }
    }
}
