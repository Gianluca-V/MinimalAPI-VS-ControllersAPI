using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FullRESTAPI.Models;

public partial class ProductsApiContext : DbContext
{
    public ProductsApiContext()
    {
    }

    public ProductsApiContext(DbContextOptions<ProductsApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductsMinimalAPI;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC072402361F");

            entity.Property(e => e.Name).HasColumnType("text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
