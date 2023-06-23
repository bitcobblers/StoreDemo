namespace Store.Api.Models;

public record LoginModel(string Username, string Password);
public record LoginResponse(string? Token, string Message);