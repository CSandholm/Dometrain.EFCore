using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Data;

public class MoviesContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    // => get only property , Set  is protected, not nullable and cleaner than ending with = null!;
    //public DbSet<Movie> Movies { set; get; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Data Source=localhost; Initial Catalog=MoviesDB; User Id=sa; Password=MySQLPassword123; TrustServerCertificate=True;");
        base.OnConfiguring(optionsBuilder);
    }
}