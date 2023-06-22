namespace Store.Api.Database;

public class ShoppingCart
{
    public int Id { get; set; }
    public Account Account { get; set; }
    public ICollection<Product> Products { get; set; }
}

