using BC.Domain.Models.Enums;
using BC.Domain.Service;

namespace BC.Api.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BCHeaderMiddleWare
    {
        private readonly RequestDelegate _next;

        public BCHeaderMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, BusinessCentralHeader header)
        {
            // Skip middleware logic for the token endpoint (or any other endpoint)
            if (httpContext.Request.Path.StartsWithSegments("/auth/token"))
            {
                await _next(httpContext);
                return;
            }

            // Check for a specific header
            if (!httpContext.Request.Headers.TryGetValue("X-Source-Header", out var headerValue))
            {
                // Short-circuit the pipeline by returning an error response
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync("X-Source-Header is missing.");
                return; // Do not call the next middleware
            }
            RequestSource sourceValue;
            if (string.IsNullOrEmpty(headerValue) || !Enum.TryParse(headerValue, out sourceValue))
            {
                // Log or process the custom header value
                Console.WriteLine($"X-Custom-Header: {headerValue}");

                // Short-circuit the pipeline by returning an error response
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync("X-Source-Header is invalid.");
                return; // Do not call the next middleware

            }

            header.Value = headerValue!;
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BCHeaderMiddleWareExtensions
    {
        public static IApplicationBuilder UseBCHeaderMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BCHeaderMiddleWare>();
        }
    }
}
