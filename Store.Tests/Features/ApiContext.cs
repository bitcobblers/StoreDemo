using Store.Api.Models;

namespace Store.Tests.Features;

public class ApiContext
{
    public HttpClient? Client { get; set; }
    public LoginRequest? Credentials { get; set; }
    public HttpResponseMessage? LastResponse { get; set; }
    public string? Token { get; set; }
}
