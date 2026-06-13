using EcoWasteApi.Models;

namespace EcoWasteApi.Repositories;

public interface IWasteRecordRepository
{
  Task<WasteRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
  Task<IReadOnlyList<WasteRecord>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
  Task<int> CountAsync(CancellationToken cancellationToken = default);
  Task<decimal> GetTotalWeightAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyList<WasteStatusSummary>> GetStatusSummaryAsync(CancellationToken cancellationToken = default);
  Task AddAsync(WasteRecord wasteRecord, CancellationToken cancellationToken = default);
  void Remove(WasteRecord wasteRecord);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public sealed record WasteStatusSummary(string Status, decimal TotalWeightKg, int TotalRecords);

