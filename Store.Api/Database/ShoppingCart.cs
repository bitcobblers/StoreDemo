namespace Store.Api.Database;

public class ShoppingCart
{
    public int Id { get; set; }
    public virtual List<CartItem> Items { get; set; } = new ();
}
