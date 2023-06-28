namespace Store.Api.Database;

public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = Guid.NewGuid().ToString();
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
