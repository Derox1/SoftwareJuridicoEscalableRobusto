using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Excepciones;
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
                var statusCode = ex switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var error = new ProblemDetails
                {
                    Status = context.Response.StatusCode,
                    Title = statusCode == 404 ? "No encontrado" : "Error interno del servidor",
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var json = JsonSerializer.Serialize(error, options);
                await context.Response.WriteAsync(json);
            }


        }
    }
}
