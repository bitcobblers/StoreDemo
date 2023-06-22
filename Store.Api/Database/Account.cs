namespace Store.Api.Database;

public class Account
{
    public int Id { get; set; }
    public Guid Uid { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public ICollection<Order> Orders {  get; set; } = new List<Order>();
}
