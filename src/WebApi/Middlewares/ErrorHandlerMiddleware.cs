using WebApi.Extensions;

namespace WebApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ApplicationException appEx)
        {
            await appEx.GenerateApplicationErrorResponse(httpContext);
        }
        catch (Exception ex)
        {
            await ex.GenerateUnhandledErrorResponse(httpContext, _logger);
        }
    }
}