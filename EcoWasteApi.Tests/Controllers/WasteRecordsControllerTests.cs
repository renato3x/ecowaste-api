using EcoWasteApi.Controllers;
using EcoWasteApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EcoWasteApi.Tests.Controllers;

public class WasteRecordsControllerTests
{
  [Fact]
  public async Task Delete_WhenCalled_ReturnsNoContent()
  {
    var serviceMock = new Mock<IWasteRecordService>();
    var controller = new WasteRecordsController(serviceMock.Object);

    var result = await controller.Delete(1, CancellationToken.None);

    Assert.IsType<NoContentResult>(result);
    serviceMock.Verify(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
  }
}

