using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Application.Common.Exceptions;

namespace OmniReserve.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió una excepción no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        if (exception is ValidationException validationEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de Validación",
                Detail = "Ocurrieron uno o más errores de validación."
            };

            problemDetails.Extensions.Add("errors", validationEx.Errors);

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var defaultProblemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error interno del servidor",
            Detail = "Ocurrió un error inesperado en el servidor."
        };

        var defaultJson = JsonSerializer.Serialize(defaultProblemDetails);
        await context.Response.WriteAsync(defaultJson);
    }
}