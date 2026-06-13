using EcoWasteApi.Models;

namespace EcoWasteApi.Repositories;

public interface IRecyclingCenterRepository
{
  Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);
  Task<RecyclingCenter?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
  Task<IReadOnlyList<RecyclingCenter>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
  Task<int> CountAsync(CancellationToken cancellationToken = default);
  Task<bool> HasWasteRecordsAsync(int recyclingCenterId, CancellationToken cancellationToken = default);
  Task AddAsync(RecyclingCenter recyclingCenter, CancellationToken cancellationToken = default);
  void Remove(RecyclingCenter recyclingCenter);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

