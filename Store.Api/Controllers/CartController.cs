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
    public IActionResult Clear([FromBody] int id)
    {
        var hasCart = _context.Carts.Any(x => x.Id == id);
        return hasCart ? Ok() : NotFound();
    }

    [HttpGet, Route("load/{id}")]
    public ActionResult<ShoppingCart> Load(int id)
    {
        var cart = _context.Carts.FirstOrDefault(x => x.Id == id);

        if(cart!=null)
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
    public IActionResult AddProduct([FromBody]AddProductRequest request)
    {
        return BadRequest();
    }
}
