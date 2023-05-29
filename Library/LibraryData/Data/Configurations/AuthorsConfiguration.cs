using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryData.Configurations;

using System.Globalization;
using Models;

public class AuthorsConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> entity)
    {
        entity.ToTable("Authors");

        entity.HasKey(e => e.Id).HasName("PK_Authors");

        entity.Property(e => e.Id)
	        .ValueGeneratedOnAdd()
            .HasColumnName("Id")
            .HasColumnType("int")
            .IsRequired(true);

        entity.Property(e => e.GivenName)
            .ValueGeneratedNever()
            .HasColumnName("GivenName")
	        .HasMaxLength(200)
	        .HasColumnType("nvarchar(200)")
            .IsRequired(true);

        entity.Property(e => e.FamilyName)
            .ValueGeneratedNever()
            .HasColumnName("FamilyName")
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)")
            .IsRequired(true);

        entity.Property(e => e.BirthDate)
            .ValueGeneratedNever()
            .HasColumnName("BirthDate")
            .HasColumnType("date")
            .IsRequired(false);
    }
}
