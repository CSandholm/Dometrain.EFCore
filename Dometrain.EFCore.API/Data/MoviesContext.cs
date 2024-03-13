using Dometrain.EFCore.API.Data.Entity_Mapping;
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
        optionsBuilder.UseSqlServer("""
            Data Source=localhost; 
            Initial Catalog=MoviesDB; 
            User Id=sa; 
            Password=MySQLPassword123; 
            TrustServerCertificate=True;
        """);
        //Not proper logging but a good view to see what EF is doing behind the scenes.
        optionsBuilder.LogTo(Console.WriteLine);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MovieMapping());
    }
}