using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcoWasteApi.Exceptions;
using EcoWasteApi.Models;
using EcoWasteApi.Repositories;
using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EcoWasteApi.Services;

public class AuthService(
  IUserRepository userRepository,
  IPasswordHasher<User> passwordHasher,
  IConfiguration configuration) : IAuthService
{
  public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
  {
    var normalizedEmail = request.Email.Trim().ToLowerInvariant();
    var existingUser = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
    if (existingUser is not null)
    {
      throw new ConflictException("Email is already registered.");
    }

    var user = new User
    {
      Name = request.Name.Trim(),
      Email = normalizedEmail,
      Role = string.IsNullOrWhiteSpace(request.Role) ? UserRoles.Operator : request.Role!.Trim(),
      CreatedAt = DateTime.UtcNow
    };

    if (!UserRoles.IsValid(user.Role))
    {
      throw new AppException("Role must be Admin or Operator.");
    }

    user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

    await userRepository.AddAsync(user, cancellationToken);
    await userRepository.SaveChangesAsync(cancellationToken);

    return ToRegisterResponse(user);
  }

  public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
  {
    var normalizedEmail = request.Email.Trim().ToLowerInvariant();
    var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken)
               ?? throw new UnauthorizedException("Invalid email or password.");

    var verification = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
    if (verification == PasswordVerificationResult.Failed)
    {
      throw new UnauthorizedException("Invalid email or password.");
    }

    var expiresInMinutes = configuration.GetValue<int?>("Jwt:ExpiresInMinutes") ?? 60;
    var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

    var token = GenerateJwtToken(user, expiresAt);

    return new LoginResponse
    {
      AccessToken = token,
      ExpiresAt = expiresAt,
      User = ToRegisterResponse(user)
    };
  }

  private string GenerateJwtToken(User user, DateTime expiresAt)
  {
    var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key was not configured.");
    var jwtIssuer = configuration["Jwt:Issuer"] ?? "EcoWasteApi";
    var jwtAudience = configuration["Jwt:Audience"] ?? "EcoWasteApiClients";

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>
    {
      new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new(JwtRegisteredClaimNames.Email, user.Email),
      new(JwtRegisteredClaimNames.UniqueName, user.Name),
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new(ClaimTypes.Name, user.Name),
      new(ClaimTypes.Role, user.Role),
      new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
      issuer: jwtIssuer,
      audience: jwtAudience,
      claims: claims,
      expires: expiresAt,
      signingCredentials: signingCredentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private static RegisterResponse ToRegisterResponse(User user)
  {
    return new RegisterResponse
    {
      Id = user.Id,
      Name = user.Name,
      Email = user.Email,
      Role = user.Role,
      CreatedAt = user.CreatedAt
    };
  }
}

