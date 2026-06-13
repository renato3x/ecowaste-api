using EcoWasteApi.Exceptions;
using EcoWasteApi.Models;
using EcoWasteApi.Repositories;
using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;

namespace EcoWasteApi.Services;

public class RecyclingCenterService(IRecyclingCenterRepository recyclingCenterRepository) : IRecyclingCenterService
{
  public async Task<RecyclingCenterResponse> CreateAsync(CreateRecyclingCenterRequest request, CancellationToken cancellationToken = default)
  {
    await EnsureUniqueNameAsync(request.Name, null, cancellationToken);

    var recyclingCenter = new RecyclingCenter
    {
      Name = request.Name.Trim(),
      Address = request.Address.Trim(),
      City = request.City.Trim(),
      State = request.State.Trim(),
      ContactPhone = request.ContactPhone.Trim(),
      CreatedAt = DateTime.UtcNow
    };

    await recyclingCenterRepository.AddAsync(recyclingCenter, cancellationToken);
    await recyclingCenterRepository.SaveChangesAsync(cancellationToken);

    return ToResponse(recyclingCenter);
  }

  public async Task<PagedResponse<RecyclingCenterResponse>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
  {
    var safePage = Math.Max(1, page);
    var safePageSize = Math.Max(1, pageSize);

    var totalRecords = await recyclingCenterRepository.CountAsync(cancellationToken);
    var items = await recyclingCenterRepository.GetPagedAsync(safePage, safePageSize, cancellationToken);

    return new PagedResponse<RecyclingCenterResponse>
    {
      Page = safePage,
      PageSize = safePageSize,
      TotalRecords = totalRecords,
      TotalPages = totalRecords == 0 ? 0 : (int)Math.Ceiling(totalRecords / (double)safePageSize),
      Items = items.Select(ToResponse).ToList()
    };
  }

  public async Task<RecyclingCenterResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    var recyclingCenter = await recyclingCenterRepository.GetByIdAsync(id, cancellationToken)
                          ?? throw new NotFoundException("Recycling center not found.");

    return ToResponse(recyclingCenter);
  }

  public async Task<RecyclingCenterResponse> UpdateAsync(int id, UpdateRecyclingCenterRequest request, CancellationToken cancellationToken = default)
  {
    var recyclingCenter = await recyclingCenterRepository.GetByIdAsync(id, cancellationToken)
                          ?? throw new NotFoundException("Recycling center not found.");

    await EnsureUniqueNameAsync(request.Name, recyclingCenter.Id, cancellationToken);

    recyclingCenter.Name = request.Name.Trim();
    recyclingCenter.Address = request.Address.Trim();
    recyclingCenter.City = request.City.Trim();
    recyclingCenter.State = request.State.Trim();
    recyclingCenter.ContactPhone = request.ContactPhone.Trim();

    await recyclingCenterRepository.SaveChangesAsync(cancellationToken);

    return ToResponse(recyclingCenter);
  }

  public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
  {
    var recyclingCenter = await recyclingCenterRepository.GetByIdAsync(id, cancellationToken)
                          ?? throw new NotFoundException("Recycling center not found.");

    var hasWasteRecords = await recyclingCenterRepository.HasWasteRecordsAsync(id, cancellationToken);
    if (hasWasteRecords)
    {
      throw new ConflictException("Cannot delete a recycling center linked to waste records.");
    }

    recyclingCenterRepository.Remove(recyclingCenter);
    await recyclingCenterRepository.SaveChangesAsync(cancellationToken);
  }

  private async Task EnsureUniqueNameAsync(string name, int? excludeId, CancellationToken cancellationToken)
  {
    if (await recyclingCenterRepository.ExistsByNameAsync(name.Trim(), excludeId, cancellationToken))
    {
      throw new ConflictException("A recycling center with the same name already exists.");
    }
  }

  private static RecyclingCenterResponse ToResponse(RecyclingCenter recyclingCenter)
  {
    return new RecyclingCenterResponse
    {
      Id = recyclingCenter.Id,
      Name = recyclingCenter.Name,
      Address = recyclingCenter.Address,
      City = recyclingCenter.City,
      State = recyclingCenter.State,
      ContactPhone = recyclingCenter.ContactPhone,
      CreatedAt = recyclingCenter.CreatedAt
    };
  }
}

