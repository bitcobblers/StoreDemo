namespace Store.Api.Models;

public record AddProductRequest(int CartId, int ProductId, int Quantity);
