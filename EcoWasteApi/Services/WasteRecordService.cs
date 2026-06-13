using EcoWasteApi.Exceptions;
using EcoWasteApi.Models;
using EcoWasteApi.Repositories;
using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;

namespace EcoWasteApi.Services;

public class WasteRecordService(
  IWasteRecordRepository wasteRecordRepository,
  IRecyclingCenterRepository recyclingCenterRepository) : IWasteRecordService
{
  private const decimal CollectionThresholdKg = 100m;

  public async Task<WasteRecordResponse> CreateAsync(CreateWasteRecordRequest request, CancellationToken cancellationToken = default)
  {
    await EnsureRecyclingCenterExistsAsync(request.RecyclingCenterId, cancellationToken);
    EnsureStatus(request.Status);

    var wasteRecord = new WasteRecord
    {
      WasteType = request.WasteType.Trim(),
      WeightKg = request.WeightKg,
      CollectionDate = request.CollectionDate,
      Location = request.Location.Trim(),
      Status = request.Status.Trim(),
      RecyclingCenterId = request.RecyclingCenterId,
      CreatedAt = DateTime.UtcNow
    };

    await wasteRecordRepository.AddAsync(wasteRecord, cancellationToken);
    await wasteRecordRepository.SaveChangesAsync(cancellationToken);

    wasteRecord = await wasteRecordRepository.GetByIdAsync(wasteRecord.Id, cancellationToken)
                  ?? throw new NotFoundException("Waste record not found.");

    return ToResponse(wasteRecord);
  }

  public async Task<PagedResponse<WasteRecordResponse>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
  {
    var safePage = Math.Max(1, page);
    var safePageSize = Math.Max(1, pageSize);

    var totalRecords = await wasteRecordRepository.CountAsync(cancellationToken);
    var items = await wasteRecordRepository.GetPagedAsync(safePage, safePageSize, cancellationToken);

    return new PagedResponse<WasteRecordResponse>
    {
      Page = safePage,
      PageSize = safePageSize,
      TotalRecords = totalRecords,
      TotalPages = totalRecords == 0 ? 0 : (int)Math.Ceiling(totalRecords / (double)safePageSize),
      Items = items.Select(ToResponse).ToList()
    };
  }

  public async Task<WasteRecordResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    var wasteRecord = await wasteRecordRepository.GetByIdAsync(id, cancellationToken)
                     ?? throw new NotFoundException("Waste record not found.");

    return ToResponse(wasteRecord);
  }

  public async Task<WasteRecordResponse> UpdateAsync(int id, UpdateWasteRecordRequest request, CancellationToken cancellationToken = default)
  {
    var wasteRecord = await wasteRecordRepository.GetByIdAsync(id, cancellationToken)
                     ?? throw new NotFoundException("Waste record not found.");

    await EnsureRecyclingCenterExistsAsync(request.RecyclingCenterId, cancellationToken);
    EnsureStatus(request.Status);

    wasteRecord.WasteType = request.WasteType.Trim();
    wasteRecord.WeightKg = request.WeightKg;
    wasteRecord.CollectionDate = request.CollectionDate;
    wasteRecord.Location = request.Location.Trim();
    wasteRecord.Status = request.Status.Trim();
    wasteRecord.RecyclingCenterId = request.RecyclingCenterId;

    await wasteRecordRepository.SaveChangesAsync(cancellationToken);

    wasteRecord = await wasteRecordRepository.GetByIdAsync(id, cancellationToken)
                 ?? throw new NotFoundException("Waste record not found.");

    return ToResponse(wasteRecord);
  }

  public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
  {
    var wasteRecord = await wasteRecordRepository.GetByIdAsync(id, cancellationToken)
                     ?? throw new NotFoundException("Waste record not found.");

    wasteRecordRepository.Remove(wasteRecord);
    await wasteRecordRepository.SaveChangesAsync(cancellationToken);
  }

  public async Task<CollectionAlertResponse> GetCollectionAlertAsync(CancellationToken cancellationToken = default)
  {
    var totalWeightKg = await wasteRecordRepository.GetTotalWeightAsync(cancellationToken);

    return new CollectionAlertResponse
    {
      TotalWeightKg = totalWeightKg,
      ThresholdKg = CollectionThresholdKg,
      NeedsCollection = totalWeightKg > CollectionThresholdKg
    };
  }

  public async Task<WasteReportResponse> GetReportAsync(CancellationToken cancellationToken = default)
  {
    var totalRecords = await wasteRecordRepository.CountAsync(cancellationToken);
    var totalWeightKg = await wasteRecordRepository.GetTotalWeightAsync(cancellationToken);
    var breakdown = await wasteRecordRepository.GetStatusSummaryAsync(cancellationToken);

    return new WasteReportResponse
    {
      TotalRecords = totalRecords,
      TotalWeightKg = totalWeightKg,
      ByStatus = breakdown.Select(x => new WasteStatusBreakdownResponse
      {
        Status = x.Status,
        TotalRecords = x.TotalRecords,
        TotalWeightKg = x.TotalWeightKg
      }).ToList()
    };
  }

  private async Task EnsureRecyclingCenterExistsAsync(int recyclingCenterId, CancellationToken cancellationToken)
  {
    var recyclingCenter = await recyclingCenterRepository.GetByIdAsync(recyclingCenterId, cancellationToken);
    if (recyclingCenter is null)
    {
      throw new NotFoundException("Recycling center not found.");
    }
  }

  private static void EnsureStatus(string status)
  {
    if (!WasteRecordStatus.IsValid(status.Trim()))
    {
      throw new AppException($"Status must be one of: {string.Join(", ", WasteRecordStatus.All)}");
    }
  }

  private static WasteRecordResponse ToResponse(WasteRecord wasteRecord)
  {
    return new WasteRecordResponse
    {
      Id = wasteRecord.Id,
      WasteType = wasteRecord.WasteType,
      WeightKg = wasteRecord.WeightKg,
      CollectionDate = wasteRecord.CollectionDate,
      Location = wasteRecord.Location,
      Status = wasteRecord.Status,
      RecyclingCenterId = wasteRecord.RecyclingCenterId,
      RecyclingCenterName = wasteRecord.RecyclingCenter?.Name ?? string.Empty,
      CreatedAt = wasteRecord.CreatedAt
    };
  }
}

