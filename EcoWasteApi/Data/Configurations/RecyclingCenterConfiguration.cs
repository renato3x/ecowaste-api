using EcoWasteApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoWasteApi.Data.Configurations;

public class RecyclingCenterConfiguration : IEntityTypeConfiguration<RecyclingCenter>
{
  public void Configure(EntityTypeBuilder<RecyclingCenter> builder)
  {
    builder.ToTable("recycling_centers");

    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id)
      .HasColumnName("id")
      .ValueGeneratedOnAdd();

    builder.Property(x => x.Name)
      .HasColumnName("name")
      .HasMaxLength(140)
      .IsRequired();

    builder.Property(x => x.Address)
      .HasColumnName("address")
      .HasMaxLength(180)
      .IsRequired();

    builder.Property(x => x.City)
      .HasColumnName("city")
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(x => x.State)
      .HasColumnName("state")
      .HasMaxLength(60)
      .IsRequired();

    builder.Property(x => x.ContactPhone)
      .HasColumnName("contact_phone")
      .HasMaxLength(30)
      .IsRequired();

    builder.Property(x => x.CreatedAt)
      .HasColumnName("created_at")
      .IsRequired();

    builder.HasIndex(x => x.Name)
      .IsUnique()
      .HasDatabaseName("ux_recycling_centers_name");
  }
}

