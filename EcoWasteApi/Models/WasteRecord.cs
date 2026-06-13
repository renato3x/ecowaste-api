namespace EcoWasteApi.Models;

public class WasteRecord
{
  public int Id { get; set; }
  public string WasteType { get; set; } = string.Empty;
  public decimal WeightKg { get; set; }
  public DateTime CollectionDate { get; set; }
  public string Location { get; set; } = string.Empty;
  public string Status { get; set; } = WasteRecordStatus.PendingCollection;
  public int RecyclingCenterId { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public RecyclingCenter? RecyclingCenter { get; set; }
}

