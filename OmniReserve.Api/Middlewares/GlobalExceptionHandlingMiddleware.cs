using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Application.Common.Exceptions;
using OmniReserve.Domain.Exceptions;

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
            _logger.LogError(ex, "Ocurrió una excepción no manejada.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        // 1. Manejo Específico: Errores de Validación (Application)
        if (exception is ValidationException validationEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var validationProblem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de Validación",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Detail = "Se enviaron datos inválidos."
            };

            validationProblem.Extensions.Add("errors", validationEx.Errors);

            await context.Response.WriteAsync(JsonSerializer.Serialize(validationProblem));
            return;
        }

        // 2. Manejo Específico: Excepciones de Reglas de Negocio (Domain)
        if (exception is DomainException domainEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var domainProblem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de Dominio",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Detail = domainEx.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(domainProblem));
            return;
        }

        // 3. Manejo Genérico: Errores no controlados
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var genericProblem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error Interno del Servidor",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = "Ha ocurrido un error inesperado al procesar la solicitud."
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(genericProblem));
    }
}