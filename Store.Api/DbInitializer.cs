using Store.Api.Database;

namespace Store.Api;

public static class DbInitializer
{
    public static void Initialize(StoreContext context)
    {
        context.Database.EnsureCreated();

        if (context.Accounts.Any())
        {
            return;
        }

        AddAccount(context, "jdoe", "John", "Doe", "password");
        AddProduct(context, "Product 1", 10m, "Product 1 description.");
        AddProduct(context, "Product 2", 15m, "Product 2 description.");
        AddProduct(context, "Product 3", 20m, "Product 3 description.");
        AddProduct(context, "Product 4", 25m, "Product 4 description.");
        AddProduct(context, "Product 5", 30m, "Product 5 description.");
    }

    private static void AddAccount(StoreContext context, string username, string firstName, string lastName, string password)
    {
        var account = new Account
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            Password = password
        };

        context.Accounts.Add(account);
        context.SaveChanges();

        AddCart(context, account);
    }

    private static void AddCart(StoreContext context, Account account)
    {
        var cart = new ShoppingCart
        {
            Account = account
        };

        context.Carts.Add(cart);
        context.SaveChanges();
    }

    private static void AddProduct(StoreContext context, string name, decimal price, string description)
    {
        var product = new Product
        {
            Name = name,
            Price = price,
            Description = description
        };

        context.Products.Add(product);
        context.SaveChanges();
    }
}
