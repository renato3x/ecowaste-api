namespace EcoWasteApi.ViewModels.Responses;

public class WasteRecordResponse
{
  public int Id { get; set; }
  public string WasteType { get; set; } = string.Empty;
  public decimal WeightKg { get; set; }
  public DateTime CollectionDate { get; set; }
  public string Location { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public int RecyclingCenterId { get; set; }
  public string RecyclingCenterName { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
}

