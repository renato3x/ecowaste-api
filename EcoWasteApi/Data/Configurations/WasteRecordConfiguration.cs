using EcoWasteApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoWasteApi.Data.Configurations;

public class WasteRecordConfiguration : IEntityTypeConfiguration<WasteRecord>
{
  public void Configure(EntityTypeBuilder<WasteRecord> builder)
  {
    builder.ToTable("waste_records");

    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id)
      .HasColumnName("id")
      .ValueGeneratedOnAdd();

    builder.Property(x => x.WasteType)
      .HasColumnName("waste_type")
      .HasMaxLength(80)
      .IsRequired();

    builder.Property(x => x.WeightKg)
      .HasColumnName("weight_kg")
      .HasPrecision(10, 2)
      .IsRequired();

    builder.Property(x => x.CollectionDate)
      .HasColumnName("collection_date")
      .IsRequired();

    builder.Property(x => x.Location)
      .HasColumnName("location")
      .HasMaxLength(180)
      .IsRequired();

    builder.Property(x => x.Status)
      .HasColumnName("status")
      .HasMaxLength(40)
      .IsRequired();

    builder.Property(x => x.RecyclingCenterId)
      .HasColumnName("recycling_center_id")
      .IsRequired();

    builder.Property(x => x.CreatedAt)
      .HasColumnName("created_at")
      .IsRequired();

    builder.HasOne(x => x.RecyclingCenter)
      .WithMany(x => x.WasteRecords)
      .HasForeignKey(x => x.RecyclingCenterId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("fk_waste_records_recycling_center_id");

    builder.HasIndex(x => x.RecyclingCenterId)
      .HasDatabaseName("ix_waste_records_recycling_center_id");
  }
}

