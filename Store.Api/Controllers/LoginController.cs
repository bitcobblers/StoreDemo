using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Store.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly byte[] _key;
    private readonly string _issuer;

    public LoginController(IConfiguration configuration)
    {
        _key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        _issuer = configuration["Jwt:Issuer"]!;
    }

    [HttpPost]
    public IActionResult Login([FromBody]LoginModel login)
    {
        var token = GenerateToken();
        return Ok(new { Token = token, Message = "success" });
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
