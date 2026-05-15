using System.Net;
using System.Text.Json;

namespace JTExpress.Api.Common;

public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail("An unexpected error occurred.");
            await context.Response.WriteAsync(JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions(JsonSerializerDefaults.Web)));
        }
    }
}
