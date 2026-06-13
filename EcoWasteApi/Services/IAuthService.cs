using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;

namespace EcoWasteApi.Services;

public interface IAuthService
{
  Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
  Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}

