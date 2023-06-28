using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Api.Database;
using Store.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Store.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly byte[] _key;
    private readonly string _issuer;
    private readonly StoreContext _context;

    public LoginController(IConfiguration configuration, StoreContext context)
    {
        _key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        _issuer = configuration["Jwt:Issuer"]!;
        _context = context;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest login)
    {
        var user = _context.Accounts.FirstOrDefault(x => x.Username == login.Username && x.Password == login.Password);

        if (user != null)
        {
            var token = GenerateToken();
            return Ok(new LoginResponse(token, "Success"));
        }
        else
        {
            return BadRequest(new LoginResponse(null, "Invalid user/password"));
        }
    }

    private string GenerateToken()
    {
        var securityKey = new SymmetricSecurityKey(_key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _issuer,
            audience: null,
            claims: null,
            expires: DateTime.UtcNow.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
