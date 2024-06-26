﻿using Dometrain.EFCore.API.Data.Entity_Mapping;
using Dometrain.EFCore.API.Data.EntityMapping;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Data;

public class MoviesContext : DbContext
{
    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base (options)
    {
        
    }
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    
    // => get only property , Set  is protected, not nullable and cleaner than ending with = null!;
    //public DbSet<Movie> Movies { set; get; } = null!;

    
    /*
    //DB options should not be configured here, instead use dependency injection, hence the constructor
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
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GenreMapping());
        modelBuilder.ApplyConfiguration(new MovieMapping());
    }
}