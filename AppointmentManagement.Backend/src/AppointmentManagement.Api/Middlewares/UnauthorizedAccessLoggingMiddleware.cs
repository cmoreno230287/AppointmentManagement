using AppointmentManagement.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppointmentManagement.Api.Middleware
{
    public class UnauthorizedAccessLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnauthorizedAccessLoggingMiddleware> _logger;

        public UnauthorizedAccessLoggingMiddleware(RequestDelegate next, ILogger<UnauthorizedAccessLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                var user = context.User?.Identity?.Name ?? "Anonymous";
                var path = context.Request.Path;
                _logger.LogWarning($"Unauthorized access attempt by {user} to {path}.");

                await context.Response.WriteAsJsonAsync(new
                {
                    Data = "",
                    Message = $"Unauthorized access.",
                    Success = false
                });
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                var user = context.User?.Identity?.Name ?? "Anonymous";
                var path = context.Request.Path;
                _logger.LogWarning($"Forbidden access attempt by {user} to {path}.");

                await context.Response.WriteAsJsonAsync(new
                {
                    Data = "",
                    Message = $"You have no permission to execute this operation.",
                    Success = false
                });
            }
        }
    }
}
