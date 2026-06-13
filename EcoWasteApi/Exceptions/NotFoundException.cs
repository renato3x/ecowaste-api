namespace EcoWasteApi.Exceptions;

public class NotFoundException(string message) : AppException(message, StatusCodes.Status404NotFound);

