using ATechnologiesAssignment.App.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ATechnologiesAssignment.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new BaseResponse
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An unexpected error occurred. Please try again later.",
                Errors = new Dictionary<string, string[]> { { "Exception", new[] { exception.Message } } }
            };

            var responseContent = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            return context.Response.WriteAsync(responseContent);
        }
    }
}