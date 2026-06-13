namespace EcoWasteApi.Exceptions;

public class AppException(string message, int statusCode = StatusCodes.Status400BadRequest) : Exception(message)
{
  public int StatusCode { get; } = statusCode;
}

