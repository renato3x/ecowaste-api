using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;

namespace EcoWasteApi.Services;

public interface IWasteRecordService
{
  Task<WasteRecordResponse> CreateAsync(CreateWasteRecordRequest request, CancellationToken cancellationToken = default);
  Task<PagedResponse<WasteRecordResponse>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
  Task<WasteRecordResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
  Task<WasteRecordResponse> UpdateAsync(int id, UpdateWasteRecordRequest request, CancellationToken cancellationToken = default);
  Task DeleteAsync(int id, CancellationToken cancellationToken = default);
  Task<CollectionAlertResponse> GetCollectionAlertAsync(CancellationToken cancellationToken = default);
  Task<WasteReportResponse> GetReportAsync(CancellationToken cancellationToken = default);
}

