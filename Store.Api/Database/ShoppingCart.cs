using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Api.Database;

[Table(nameof(ShoppingCart))]
public class ShoppingCart
{
    public int Id { get; set; }
    public Account Account { get; set; }
    public ICollection<CartItem> Items { get; set; }
}
