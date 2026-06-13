using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;

namespace EcoWasteApi.Services;

public interface IRecyclingCenterService
{
  Task<RecyclingCenterResponse> CreateAsync(CreateRecyclingCenterRequest request, CancellationToken cancellationToken = default);
  Task<PagedResponse<RecyclingCenterResponse>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
  Task<RecyclingCenterResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
  Task<RecyclingCenterResponse> UpdateAsync(int id, UpdateRecyclingCenterRequest request, CancellationToken cancellationToken = default);
  Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

