namespace Store.Api.Database;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime DateOrdered { get; set; }
    public virtual List<PurchaseItem> Items { get; set; } = new();
}
