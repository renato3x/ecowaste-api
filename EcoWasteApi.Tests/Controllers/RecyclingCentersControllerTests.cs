using EcoWasteApi.Controllers;
using EcoWasteApi.Services;
using EcoWasteApi.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EcoWasteApi.Tests.Controllers;

public class RecyclingCentersControllerTests
{
  [Fact]
  public async Task GetById_WhenCenterExists_ReturnsOkResult()
  {
    var serviceMock = new Mock<IRecyclingCenterService>();
    serviceMock
      .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
      .ReturnsAsync(new RecyclingCenterResponse { Id = 1, Name = "Center" });

    var controller = new RecyclingCentersController(serviceMock.Object);

    var result = await controller.GetById(1, CancellationToken.None);

    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var payload = Assert.IsType<RecyclingCenterResponse>(okResult.Value);
    Assert.Equal(1, payload.Id);
  }
}

