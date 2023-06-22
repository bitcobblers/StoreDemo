using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly byte[] _key;
    private readonly string _issuer;

    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        _issuer = _configuration["Jwt:Issuer"]!;
    }

    [AllowAnonymous]
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
            _issuer,
            null,
            expires: DateTime.UtcNow.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
