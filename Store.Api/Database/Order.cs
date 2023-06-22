namespace Store.Api.Database;

public class Order
{
    public int Id { get; set; }
    public Guid OrderNumber { get; set; } = Guid.NewGuid();
    public DateTime DateOrdered { get; set; }
    public ICollection<PurchaseItem> Items { get; set; }
}
