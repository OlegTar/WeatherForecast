
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PowerTestTask.Server.Middlewares;

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);            
            logger.LogError(ex, ex.Message);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Detail = "Internal Server Error",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1"
        };
        var json = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(json);
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseException(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}
