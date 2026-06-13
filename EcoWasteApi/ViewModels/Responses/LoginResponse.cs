namespace EcoWasteApi.ViewModels.Responses;

public class LoginResponse
{
  public string AccessToken { get; set; } = string.Empty;
  public string TokenType { get; set; } = "Bearer";
  public DateTime ExpiresAt { get; set; }
  public RegisterResponse User { get; set; } = new();
}

