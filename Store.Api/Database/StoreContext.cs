using Microsoft.EntityFrameworkCore;

namespace Store.Api.Database;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<ShoppingCart> Carts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().ToTable("Accounts");
        modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCarts");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Order>().ToTable("Orders");
    }
}
