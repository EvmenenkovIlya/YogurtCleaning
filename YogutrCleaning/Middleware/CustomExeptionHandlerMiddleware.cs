using YogurtCleaning.Business.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace YogurtCleaning.API.Middleware;

public class CustomExeptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExeptionHandlerMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntityNotFoundException exception)
        {
            await HandleExceptionAsync(context, exception);
        }
        catch (UniquenessException exception)
        {
            await HandleExceptionAsync(context, exception);
        }
        catch (DataException exception)
        {
            await HandleExceptionAsync(context, exception);
        }
        catch (AccessException exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.Message);
                break;
            case EntityNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case UniquenessException:
                code = HttpStatusCode.UnprocessableEntity;
                break;
            case DataException:
                code = HttpStatusCode.UnprocessableEntity;
                break;
            case AccessException:
                code = HttpStatusCode.Forbidden;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }

        return context.Response.WriteAsync(result);
    }
}