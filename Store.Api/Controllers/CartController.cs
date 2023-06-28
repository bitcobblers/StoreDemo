using Microsoft.AspNetCore.Mvc;
using Store.Api.Database;
using Store.Api.Models;

namespace Store.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly StoreContext _context;

    public CartController(StoreContext context) => _context = context;

    [HttpPost("clear")]
    public async Task<IActionResult> Clear([FromBody] int id)
    {
        var cart = await _context.Carts.FindAsync(id);

        if(cart == null)
        {
            return NotFound();
        }

        cart.Items.Clear();
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet, Route("load/{id}")]
    public async Task<ActionResult<ShoppingCart>> Load(int id)
    {
        var cart = await _context.Carts.FindAsync(id);

        if (cart != null)
        {
            return Ok(cart);
        }

        return NotFound();
    }

    [HttpGet("new")]
    public ActionResult<int> New()
    {
        var cart = new ShoppingCart();

        _context.Carts.Add(cart);
        _context.SaveChanges();

        return cart.Id;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
    {
        var cart = await _context.Carts.FindAsync(request.CartId);

        if (cart == null)
        {
            return NotFound("Could not find cart.");
        }

        var addedItem = cart.Items.FirstOrDefault(x => x.Product.Id == request.ProductId);

        if (addedItem != null)
        {
            addedItem.Quantity += request.Quantity;
        }
        else
        {
            var product = await _context.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                return BadRequest("Could not find product");
            }

            cart.Items.Add(new CartItem
            {
                Product = product,
                Quantity = request.Quantity
            });
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}
