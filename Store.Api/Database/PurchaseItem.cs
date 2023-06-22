using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Api.Database;

[Table(nameof(PurchaseItem))]
public class PurchaseItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ProductIdentifier { get; set; }
}