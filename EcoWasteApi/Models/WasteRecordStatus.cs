namespace EcoWasteApi.Models;

public static class WasteRecordStatus
{
  public const string PendingCollection = "PendingCollection";
  public const string Collected = "Collected";
  public const string Processed = "Processed";

  private static readonly HashSet<string> AllowedStatuses =
  [
    PendingCollection,
    Collected,
    Processed
  ];

  public static IReadOnlyCollection<string> All => AllowedStatuses;

  public static bool IsValid(string status)
  {
    return AllowedStatuses.Contains(status);
  }
}

