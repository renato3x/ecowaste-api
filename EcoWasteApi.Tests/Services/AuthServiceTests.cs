using System.Collections.Generic;
using EcoWasteApi.Exceptions;
using EcoWasteApi.Models;
using EcoWasteApi.Repositories;
using EcoWasteApi.Services;
using EcoWasteApi.ViewModels.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EcoWasteApi.Tests.Services;

public class AuthServiceTests
{
  [Fact]
  public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsConflictException()
  {
    var userRepositoryMock = new Mock<IUserRepository>();
    var passwordHasher = new PasswordHasher<User>();
    var configuration = BuildConfiguration();

    userRepositoryMock
      .Setup(x => x.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>()))
      .ReturnsAsync(new User { Email = "john@example.com" });

    var service = new AuthService(userRepositoryMock.Object, passwordHasher, configuration);

    await Assert.ThrowsAsync<ConflictException>(() => service.RegisterAsync(new RegisterRequest
    {
      Name = "John",
      Email = "john@example.com",
      Password = "123456"
    }));
  }

  [Fact]
  public async Task LoginAsync_WhenCredentialsAreValid_ReturnsToken()
  {
    var userRepositoryMock = new Mock<IUserRepository>();
    var passwordHasher = new PasswordHasher<User>();
    var configuration = BuildConfiguration();

    var user = new User
    {
      Id = 1,
      Name = "John",
      Email = "john@example.com",
      Role = UserRoles.Admin,
      CreatedAt = DateTime.UtcNow
    };

    user.PasswordHash = passwordHasher.HashPassword(user, "123456");

    userRepositoryMock
      .Setup(x => x.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>()))
      .ReturnsAsync(user);

    var service = new AuthService(userRepositoryMock.Object, passwordHasher, configuration);

    var response = await service.LoginAsync(new LoginRequest
    {
      Email = "john@example.com",
      Password = "123456"
    });

    Assert.False(string.IsNullOrWhiteSpace(response.AccessToken));
    Assert.Equal("john@example.com", response.User.Email);
  }

  private static IConfiguration BuildConfiguration()
  {
    var values = new Dictionary<string, string?>
    {
      ["Jwt:Key"] = "THIS_IS_A_DEMO_KEY_WITH_MORE_THAN_32_CHARACTERS",
      ["Jwt:Issuer"] = "EcoWasteApi",
      ["Jwt:Audience"] = "EcoWasteApiClients",
      ["Jwt:ExpiresInMinutes"] = "60"
    };

    return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
  }
}

