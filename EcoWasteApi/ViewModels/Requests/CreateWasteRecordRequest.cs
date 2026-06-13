namespace EcoWasteApi.ViewModels.Requests;

public class CreateWasteRecordRequest
{
  public string WasteType { get; set; } = string.Empty;
  public decimal WeightKg { get; set; }
  public DateTime CollectionDate { get; set; }
  public string Location { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public int RecyclingCenterId { get; set; }
}

