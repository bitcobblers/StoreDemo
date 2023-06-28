namespace Store.Api.Database;

public class PurchaseItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int ProductId { get; set; }
}