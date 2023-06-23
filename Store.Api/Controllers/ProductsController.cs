using Microsoft.AspNetCore.Mvc;
using Store.Api.Database;

namespace Store.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly StoreContext _context;

    public ProductsController(StoreContext context) => _context = context;

    [HttpGet]
    public ActionResult<Product[]> Get() => Ok(_context.Products.ToArray());
}
