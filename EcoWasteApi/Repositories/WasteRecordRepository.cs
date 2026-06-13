using EcoWasteApi.Data;
using EcoWasteApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoWasteApi.Repositories;

public class WasteRecordRepository(AppDbContext dbContext) : IWasteRecordRepository
{
  public Task<WasteRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    return dbContext.WasteRecords
      .Include(x => x.RecyclingCenter)
      .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
  }

  public async Task<IReadOnlyList<WasteRecord>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
  {
    return await dbContext.WasteRecords
      .AsNoTracking()
      .Include(x => x.RecyclingCenter)
      .OrderByDescending(x => x.CreatedAt)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync(cancellationToken);
  }

  public Task<int> CountAsync(CancellationToken cancellationToken = default)
  {
    return dbContext.WasteRecords.CountAsync(cancellationToken);
  }

  public async Task<decimal> GetTotalWeightAsync(CancellationToken cancellationToken = default)
  {
    return await dbContext.WasteRecords.SumAsync(x => (decimal?)x.WeightKg, cancellationToken) ?? 0m;
  }

  public async Task<IReadOnlyList<WasteStatusSummary>> GetStatusSummaryAsync(CancellationToken cancellationToken = default)
  {
    var rows = await dbContext.WasteRecords
      .AsNoTracking()
      .GroupBy(x => x.Status)
      .Select(x => new
      {
        Status = x.Key,
        TotalWeightKg = x.Sum(y => (decimal?)y.WeightKg) ?? 0m,
        TotalRecords = x.Count()
      })
      .OrderBy(x => x.Status)
      .ToListAsync(cancellationToken);

    return rows
      .Select(x => new WasteStatusSummary(x.Status, x.TotalWeightKg, x.TotalRecords))
      .ToList();
  }

  public async Task AddAsync(WasteRecord wasteRecord, CancellationToken cancellationToken = default)
  {
    await dbContext.WasteRecords.AddAsync(wasteRecord, cancellationToken);
  }

  public void Remove(WasteRecord wasteRecord)
  {
    dbContext.WasteRecords.Remove(wasteRecord);
  }

  public Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return dbContext.SaveChangesAsync(cancellationToken);
  }
}

