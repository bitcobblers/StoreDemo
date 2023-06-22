using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Api.Database;

[Table(nameof(CartItem))]
public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }
}
