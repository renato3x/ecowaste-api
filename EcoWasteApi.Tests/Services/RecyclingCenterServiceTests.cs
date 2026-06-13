using EcoWasteApi.Exceptions;
using EcoWasteApi.Repositories;
using EcoWasteApi.Services;
using EcoWasteApi.ViewModels.Requests;
using Moq;

namespace EcoWasteApi.Tests.Services;

public class RecyclingCenterServiceTests
{
  [Fact]
  public async Task CreateAsync_WhenNameAlreadyExists_ThrowsConflictException()
  {
    var repositoryMock = new Mock<IRecyclingCenterRepository>();
    repositoryMock
      .Setup(x => x.ExistsByNameAsync("Center 1", null, It.IsAny<CancellationToken>()))
      .ReturnsAsync(true);

    var service = new RecyclingCenterService(repositoryMock.Object);

    await Assert.ThrowsAsync<ConflictException>(() => service.CreateAsync(new CreateRecyclingCenterRequest
    {
      Name = "Center 1",
      Address = "Street",
      City = "City",
      State = "State",
      ContactPhone = "11999999999"
    }));
  }
}

