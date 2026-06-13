using EcoWasteApi.Data;
using EcoWasteApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoWasteApi.Repositories;

public class RecyclingCenterRepository(AppDbContext dbContext) : IRecyclingCenterRepository
{
  public Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
  {
    var query = dbContext.RecyclingCenters.AsQueryable();

    if (excludeId.HasValue)
    {
      query = query.Where(x => x.Id != excludeId.Value);
    }

    return query.AnyAsync(x => x.Name == name, cancellationToken);
  }

  public Task<RecyclingCenter?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    return dbContext.RecyclingCenters.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
  }

  public async Task<IReadOnlyList<RecyclingCenter>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
  {
    return await dbContext.RecyclingCenters
      .AsNoTracking()
      .OrderByDescending(x => x.CreatedAt)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync(cancellationToken);
  }

  public Task<int> CountAsync(CancellationToken cancellationToken = default)
  {
    return dbContext.RecyclingCenters.CountAsync(cancellationToken);
  }

  public Task<bool> HasWasteRecordsAsync(int recyclingCenterId, CancellationToken cancellationToken = default)
  {
    return dbContext.WasteRecords.AnyAsync(x => x.RecyclingCenterId == recyclingCenterId, cancellationToken);
  }

  public async Task AddAsync(RecyclingCenter recyclingCenter, CancellationToken cancellationToken = default)
  {
    await dbContext.RecyclingCenters.AddAsync(recyclingCenter, cancellationToken);
  }

  public void Remove(RecyclingCenter recyclingCenter)
  {
    dbContext.RecyclingCenters.Remove(recyclingCenter);
  }

  public Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return dbContext.SaveChangesAsync(cancellationToken);
  }
}

