using Microsoft.AspNetCore.Mvc;
using Store.Api.Database;
using Store.Api.Models;

namespace Store.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly StoreContext _context;

    public OrderController(StoreContext context) => _context = context;

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Load(string id)
    {
        var order = _context.Orders.FirstOrDefault(x => x.OrderNumber == id);

        if(order == null)
        {
            return NotFound("Order not found");
        }

        return Ok(order);
    }


    [HttpPost("place")]
    public async Task<ActionResult<PlaceOrderResponse>> PlaceOrder([FromBody] PlaceOrderRequest order)
    {
        var cart = await _context.Carts.FindAsync(order.CartId);

        if (cart == null)
        {
            return BadRequest(new PlaceOrderResponse(null, "Unable to find cart"));
        }

        var newOrder = new Order
        {
            DateOrdered = DateTime.UtcNow,
            Items = (from item in cart.Items
                     select new PurchaseItem
                     {
                         Price = item.Product.Price,
                         ProductId = item.Product.Id,
                         Quantity = item.Quantity
                     }).ToList()
        };

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();

        return Ok(new PlaceOrderResponse(newOrder.OrderNumber, "Success"));
    }
}
