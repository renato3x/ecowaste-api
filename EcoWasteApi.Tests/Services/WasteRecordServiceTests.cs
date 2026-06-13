using EcoWasteApi.Repositories;
using EcoWasteApi.Services;
using Moq;

namespace EcoWasteApi.Tests.Services;

public class WasteRecordServiceTests
{
  [Fact]
  public async Task GetCollectionAlertAsync_WhenTotalWeightIsAboveThreshold_ReturnsNeedsCollectionTrue()
  {
    var wasteRepositoryMock = new Mock<IWasteRecordRepository>();
    var recyclingCenterRepositoryMock = new Mock<IRecyclingCenterRepository>();

    wasteRepositoryMock
      .Setup(x => x.GetTotalWeightAsync(It.IsAny<CancellationToken>()))
      .ReturnsAsync(150m);

    var service = new WasteRecordService(wasteRepositoryMock.Object, recyclingCenterRepositoryMock.Object);

    var response = await service.GetCollectionAlertAsync();

    Assert.True(response.NeedsCollection);
    Assert.Equal(150m, response.TotalWeightKg);
    Assert.Equal(100m, response.ThresholdKg);
  }
}

