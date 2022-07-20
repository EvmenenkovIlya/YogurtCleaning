using YogurtCleaning.Business.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;



namespace YogurtCleaning.Middleware;

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
            await HandleExceptionAsync(context, exception, HttpStatusCode.NotFound);
        }
        catch (UniquenessException exception)
        {
            await HandleExceptionAsync(context, exception, HttpStatusCode.UnprocessableEntity);
        }
        catch (DataException exception)
        {
            await HandleExceptionAsync(context, exception, HttpStatusCode.UnprocessableEntity);
        }
        catch (AccessException exception)
        {
            await HandleExceptionAsync(context, exception, HttpStatusCode.Forbidden);
        }
        catch (BadRequestException exception)
        {
            await HandleExceptionAsync(context, exception, HttpStatusCode.BadRequest);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception, HttpStatusCode.InternalServerError);
        }

    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
    {
        var result = string.Empty;      
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }

        return context.Response.WriteAsync(result);
    }
}
