using EcoWasteApi.Controllers;
using EcoWasteApi.Services;
using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EcoWasteApi.Tests.Controllers;

public class AuthControllerTests
{
  [Fact]
  public async Task Register_WhenRequestIsValid_ReturnsCreatedResponse()
  {
    var serviceMock = new Mock<IAuthService>();
    serviceMock
      .Setup(x => x.RegisterAsync(It.IsAny<RegisterRequest>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(new RegisterResponse { Id = 1, Email = "john@example.com", Name = "John", Role = "Operator" });

    var controller = new AuthController(serviceMock.Object);

    var result = await controller.Register(new RegisterRequest
    {
      Name = "John",
      Email = "john@example.com",
      Password = "123456"
    }, CancellationToken.None);

    var createdResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
  }
}


