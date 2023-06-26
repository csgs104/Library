namespace LibraryData.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class BooksConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> entity)
    {
        entity.ToTable("Books");

        entity.HasKey(e => e.Id).HasName("PK_Books");
        entity.HasAlternateKey(e => e.ISBN);

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("Id")
            .HasColumnType("int")
            .IsRequired(true);

        entity.Property(e => e.ISBN)
            .ValueGeneratedNever()
            .HasColumnName("ISBN")
            .HasMaxLength(30)
            .HasColumnType("nvarchar(30)")
            .IsRequired(true);

        entity.Property(e => e.Title)
            .ValueGeneratedNever()
            .HasColumnName("Title")
            .HasMaxLength(200)
            .HasColumnType("nvarchar(400)")
            .IsRequired(true);

        entity.Property(e => e.PublicationDate)
            .ValueGeneratedNever()
            .HasColumnName("PublicationDate")
            .HasColumnType("date")
            .IsRequired(false);

        entity.HasOne(d => d.Author).WithMany(p => p.Books)
            .HasForeignKey(d => d.AuthorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Author_Id");
    }
}