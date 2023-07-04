using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Database;
using Store.Api.Models;
using System.Net.Http.Json;

namespace Store.Tests.Steps;

public static class CartSteps
{
    private static readonly Random random = new();

    public static LambdaStep NewCart(HttpClient client) =>
        new LambdaStep("Create new cart")
            .HandleAsync(async context =>
            {
                var url = $"api/cart/new";
                var response = await client.GetStringAsync(url);

                context.CartId = int.Parse(response);
            });

    public static LambdaStep LoadProducts(HttpClient client) =>
        new LambdaStep("Get product list")
            .HandleAsync(async context =>
            {
                var url = "api/products";
                var response = await client.GetFromJsonAsync<Product[]>(url);

                context.Products = response;
            });

    // ---

    public static LambdaStep AddRandomProductToCart(HttpClient client) =>
        new LambdaStep("Add random product to cart")
            .HandleAsync(async context =>
            {
                var cartId = (int)context.CartId;
                var products = (Product[])context.Products;
                var product = products[random.Next(0, products.Length)];

                var url = "api/cart/add";
                await client.PostAsJsonAsync(url, new AddProductRequest(cartId, product.Id, 1));
            });

    public static LambdaStep LoadCurrentCart(HttpClient client) =>
        new LambdaStep("Load current cart")
            .HandleAsync(async context =>
            {
                var cartId = (int)context.CartId;
                var url = $"api/cart/load/{cartId}";

                try
                {
                    context.Cart = await client.GetFromJsonAsync<ShoppingCart>(url);
                }
                catch (HttpRequestException)
                {
                    context.Cart = null;
                }
            });

    public static LambdaStep ClearCurrentCart(HttpClient client) =>
        new LambdaStep("Clear current cart")
            .HandleAsync(async context =>
            {
                var cartId = (int)context.CartId;
                var url = "api/cart/clear";

                await client.PostAsJsonAsync(url, cartId);
            });

    // ---

    public static LambdaStep CheckCartSize(int size) =>
        new WhenLambdaStep($"Check cart contains {size:N0} items")
            .Handle(context =>
            {
                var cart = (ShoppingCart)context.Cart;
                Assert.Equal(size, cart.Items.Count);
            });
}