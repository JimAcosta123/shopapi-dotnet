using Microsoft.EntityFrameworkCore;
using ShopApi.Models;

namespace ShopApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderItem>()
            .Property(i => i.UnitPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Order>()
            .Property(o => o.Total)
            .HasColumnType("decimal(18,2)");

        // Datos semilla: categorias y productos de ejemplo
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronica" },
            new Category { Id = 2, Name = "Libros" },
            new Category { Id = 3, Name = "Hogar" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Mouse inalambrico", Description = "Mouse ergonomico 2.4GHz", Price = 39.90m, Stock = 50, CategoryId = 1 },
            new Product { Id = 2, Name = "Teclado mecanico", Description = "Switches azules, retroiluminado", Price = 129.90m, Stock = 25, CategoryId = 1 },
            new Product { Id = 3, Name = "Clean Architecture", Description = "Libro de Robert C. Martin", Price = 89.00m, Stock = 15, CategoryId = 2 },
            new Product { Id = 4, Name = "Organizador de escritorio", Description = "Madera, 5 compartimentos", Price = 59.90m, Stock = 30, CategoryId = 3 }
        );
    }
}
