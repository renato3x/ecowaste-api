namespace EcoWasteApi.Models;

public static class UserRoles
{
  public const string Admin = "Admin";
  public const string Operator = "Operator";

  public static bool IsValid(string role)
  {
    return role is Admin or Operator;
  }
}

