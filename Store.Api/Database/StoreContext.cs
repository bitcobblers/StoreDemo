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
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().ToTable("Accounts");
        modelBuilder.Entity<ShoppingCart>().ToTable("Carts");
        modelBuilder.Entity<CartItem>().ToTable("CartItems");
        modelBuilder.Entity<PurchaseItem>().ToTable("PurchaseItems");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Order>().ToTable("Orders");
    }
}
