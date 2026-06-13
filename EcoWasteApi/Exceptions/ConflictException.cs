namespace EcoWasteApi.Exceptions;

public class ConflictException(string message) : AppException(message, StatusCodes.Status409Conflict);

