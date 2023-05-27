using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using LibraryData.Models;

namespace LibraryData.Data;

public partial class LibraryContext : DbContext
{
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;

    public LibraryContext(DbContextOptions<LibraryContext> options)
    : base(options)
    { }

    public LibraryContext()
    { }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //   => optionsBuilder.UseSqlServer("Connection String");

    // protected override void OnModelCreating(ModelBuilder modelBuilder) 
    // { modelBuilder.ApplyConfiguration(new MessagesConfiguration()); modelBuilder.ApplyConfiguration(new AuthorsConfiguration()); }
}




// ADD BEFORE THE INITIAL MIGRATION 
// REMOVE AFTER THE INITIAL MIGRATION

public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
{
    public LibraryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
        optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=Library;User ID=SA;Password=R00t.r00T;Encrypt=True;TrustServerCertificate=True;Connection Timeout=180;");

        return new LibraryContext(optionsBuilder.Options);
    }
}
