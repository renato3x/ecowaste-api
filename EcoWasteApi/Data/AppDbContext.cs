using EcoWasteApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoWasteApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<User> Users => Set<User>();
  public DbSet<RecyclingCenter> RecyclingCenters => Set<RecyclingCenter>();
  public DbSet<WasteRecord> WasteRecords => Set<WasteRecord>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}

