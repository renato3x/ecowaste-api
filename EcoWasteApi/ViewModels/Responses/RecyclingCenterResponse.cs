namespace EcoWasteApi.ViewModels.Responses;

public class RecyclingCenterResponse
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public string State { get; set; } = string.Empty;
  public string ContactPhone { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
}

