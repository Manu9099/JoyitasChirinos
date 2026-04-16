using JoyitasChirinos.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;
using NotFoundException = JoyitasChirinos.Application.Common.Exceptions.NotFoundException;
namespace JoyitasChirinos.API.Middleware;
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        try { await next(ctx); }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
            await HandleExceptionAsync(ctx, ex);
        }
    }
    private static Task HandleExceptionAsync(HttpContext ctx, Exception ex)
    {
        var (status, msg, errors) = ex switch
        {
            NotFoundException nf    => (HttpStatusCode.NotFound, nf.Message, (object?)null),
            ValidationException ve  => (HttpStatusCode.BadRequest, "Errores de validación", (object?)ve.Errores),
            _                       => (HttpStatusCode.InternalServerError, "Error interno del servidor", (object?)null)
        };
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)status;
        var body = JsonSerializer.Serialize(new { status = (int)status, message = msg, errors });
        return ctx.Response.WriteAsync(body);
    }
}
