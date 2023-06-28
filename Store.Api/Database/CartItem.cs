namespace Store.Api.Database;

public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public virtual Product? Product { get; set; }
}
