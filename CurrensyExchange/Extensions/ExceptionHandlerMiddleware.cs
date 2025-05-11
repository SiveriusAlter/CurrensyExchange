using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.API.Extensions;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var statusCode = (int)HttpStatusCode.OK;

        try
        {
            await _next.Invoke(context);
        }
        catch (ArgumentException ex)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            await HandleExceptionMessageAsync(context, ex, statusCode).ConfigureAwait(false);
        }
        catch (InvalidOperationException ex)
        {
            statusCode = (int)HttpStatusCode.Conflict;
            await HandleExceptionMessageAsync(context, ex, statusCode).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
            await HandleExceptionMessageAsync(context, ex, statusCode).ConfigureAwait(false);
        }
    }


    private static Task HandleExceptionMessageAsync(HttpContext context, Exception ex, int statusCode)
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.Headers.ContentLanguage = "ru-RU";
        var result = JsonSerializer.Serialize(new
        {
            StatusCode = statusCode,
            ErrorMessage = ex.Message
        }, 
            new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            });
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}