namespace EcoWasteApi.ViewModels.Responses;

public class CollectionAlertResponse
{
  public bool NeedsCollection { get; set; }
  public decimal TotalWeightKg { get; set; }
  public decimal ThresholdKg { get; set; }
}

