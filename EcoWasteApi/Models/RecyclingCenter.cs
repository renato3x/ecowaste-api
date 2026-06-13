namespace EcoWasteApi.Models;

public class RecyclingCenter
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public string State { get; set; } = string.Empty;
  public string ContactPhone { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public ICollection<WasteRecord> WasteRecords { get; set; } = new List<WasteRecord>();
}

