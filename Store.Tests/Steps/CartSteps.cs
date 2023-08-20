using DrillSergeant;
using Store.Api.Database;
using Store.Api.Models;
using System.Net.Http.Json;

namespace Store.Tests.Steps;

public static class CartSteps
{
    private static readonly Random random = new();

    public static LambdaStep NewCart(HttpClient client) =>
        new LambdaStep("Create new cart")
            .HandleAsync(async () =>
            {
                var url = $"api/cart/new";
                var response = await client.GetStringAsync(url);

                CurrentBehavior.Context.CartId = int.Parse(response);
            });

    public static LambdaStep LoadProducts(HttpClient client) =>
        new LambdaStep("Get product list")
            .HandleAsync(async () =>
            {
                var url = "api/products";
                var response = await client.GetFromJsonAsync<Product[]>(url);

                CurrentBehavior.Context.Products = response;
            });

    // ---

    public static LambdaStep AddRandomProductToCart(HttpClient client) =>
        new LambdaStep("Add random product to cart")
            .HandleAsync(async () =>
            {
                var cartId = (int)CurrentBehavior.Context.CartId;
                var products = (Product[])CurrentBehavior.Context.Products;
                var product = products[random.Next(0, products.Length)];

                var url = "api/cart/add";
                await client.PostAsJsonAsync(url, new AddProductRequest(cartId, product.Id, 1));
            });

    public static LambdaStep LoadCurrentCart(HttpClient client) =>
        new LambdaStep("Load current cart")
            .HandleAsync(async () =>
            {
                var cartId = (int)CurrentBehavior.Context.CartId;
                var url = $"api/cart/load/{cartId}";

                try
                {
                    CurrentBehavior.Context.Cart = await client.GetFromJsonAsync<ShoppingCart>(url);
                }
                catch (HttpRequestException)
                {
                    CurrentBehavior.Context.Cart = null;
                }
            });

    public static LambdaStep ClearCurrentCart(HttpClient client) =>
        new LambdaStep("Clear current cart")
            .HandleAsync(async () =>
            {
                var cartId = (int)CurrentBehavior.Context.CartId;
                var url = "api/cart/clear";

                await client.PostAsJsonAsync(url, cartId);
            });

    // ---

    public static LambdaStep CheckCartSize(int size) =>
        new LambdaStep($"Check cart contains {size:N0} items")
            .Handle(() =>
            {
                var cart = (ShoppingCart)CurrentBehavior.Context.Cart;
                Assert.Equal(size, cart.Items.Count);
            });
}