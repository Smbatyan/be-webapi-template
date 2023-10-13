using System.Net;
using System.Net.Mime;
using Core.Exceptions;
using WebApi.DTO.Response;
using WebApi.Middlewares;

namespace WebApi.Extensions;

internal static class ErrorResponseExtension
{
    private const string ErrorMessage = "Something went wrong";

    internal static async Task GenerateApplicationErrorResponse(this ApplicationException httpResponseException,
        HttpContext httpContext)
    {
        ApplicationExceptionBase exception = httpResponseException as ApplicationExceptionBase;
        httpContext.Response.StatusCode = exception!.StatusCode;

        ErrorResponse error = new() { Message = httpResponseException.Message };

        await httpContext.Response.WriteAsJsonAsync(error);
    }

    internal static async Task GenerateUnhandledErrorResponse(this Exception exception,
        HttpContext httpContext, ILogger<ErrorHandlerMiddleware> logger)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        
        ErrorResponse errorResponse = new() { Message = ErrorMessage };

        logger.LogError(exception.ToFullBlownString()); // Add user info here

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(errorResponse);
    }
}