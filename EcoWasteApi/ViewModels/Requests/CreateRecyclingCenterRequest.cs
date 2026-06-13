namespace EcoWasteApi.ViewModels.Requests;

public class CreateRecyclingCenterRequest
{
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public string State { get; set; } = string.Empty;
  public string ContactPhone { get; set; } = string.Empty;
}

