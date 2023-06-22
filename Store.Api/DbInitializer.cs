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

    private static ShoppingCart AddCart(StoreContext context, Account account)
    {
        var cart = new ShoppingCart
        {
            Account = account
        };

        context.Carts.Add(cart);
        context.SaveChanges();

        return cart;
    }
}
