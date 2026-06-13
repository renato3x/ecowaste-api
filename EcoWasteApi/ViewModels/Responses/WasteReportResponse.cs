namespace EcoWasteApi.ViewModels.Responses;

public class WasteReportResponse
{
  public int TotalRecords { get; set; }
  public decimal TotalWeightKg { get; set; }
  public IReadOnlyList<WasteStatusBreakdownResponse> ByStatus { get; set; } = [];
}

public class WasteStatusBreakdownResponse
{
  public string Status { get; set; } = string.Empty;
  public int TotalRecords { get; set; }
  public decimal TotalWeightKg { get; set; }
}

