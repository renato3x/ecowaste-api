using EcoWasteApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoWasteApi.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.ToTable("users");

    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id)
      .HasColumnName("id")
      .ValueGeneratedOnAdd();

    builder.Property(x => x.Name)
      .HasColumnName("name")
      .HasMaxLength(120)
      .IsRequired();

    builder.Property(x => x.Email)
      .HasColumnName("email")
      .HasMaxLength(180)
      .IsRequired();

    builder.Property(x => x.PasswordHash)
      .HasColumnName("password_hash")
      .HasMaxLength(512)
      .IsRequired();

    builder.Property(x => x.Role)
      .HasColumnName("role")
      .HasMaxLength(50)
      .IsRequired();

    builder.Property(x => x.CreatedAt)
      .HasColumnName("created_at")
      .IsRequired();

    builder.HasIndex(x => x.Email)
      .IsUnique()
      .HasDatabaseName("ux_users_email");
  }
}

