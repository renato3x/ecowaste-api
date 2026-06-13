using System.Text.Json;
using EcoWasteApi.Exceptions;
using FluentValidation;

namespace EcoWasteApi.Middlewares;

public class GlobalExceptionMiddleware(
  RequestDelegate next,
  ILogger<GlobalExceptionMiddleware> logger,
  IHostEnvironment environment)
{
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await next(context);
    }
    catch (Exception exception)
    {
      await HandleExceptionAsync(context, exception);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    logger.LogError(exception, "Unhandled exception captured by middleware.");

    var statusCode = exception switch
    {
      AppException appException => appException.StatusCode,
      ValidationException => StatusCodes.Status400BadRequest,
      _ => StatusCodes.Status500InternalServerError
    };

    var errors = exception switch
    {
      ValidationException validationException => validationException.Errors
        .GroupBy(x => x.PropertyName)
        .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).Distinct()),
      _ => null
    };

    var response = new
    {
      message = statusCode == StatusCodes.Status500InternalServerError && !environment.IsDevelopment()
        ? "An unexpected error occurred."
        : exception.Message,
      trace_id = context.TraceIdentifier,
      errors
    };

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = statusCode;

    await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
      DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower
    }));
  }
}

