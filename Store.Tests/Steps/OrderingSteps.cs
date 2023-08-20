using DrillSergeant;
using Store.Api.Database;
using Store.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace Store.Tests.Steps;

public static class OrderingSteps
{
    public static LambdaStep PlaceOrder(HttpClient client) =>
        new LambdaStep("Place order")
            .HandleAsync(async () =>
            {
                var cartId = (int)CurrentBehavior.Context.CartId;
                var order = new PlaceOrderRequest(cartId);
                var url = "api/order/place";

                var response = await client.PostAsJsonAsync(url, order);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var body = await response
                        .Content
                        .ReadFromJsonAsync<PlaceOrderResponse>();

                    CurrentBehavior.Context.OrderId = body?.OrderNumber;
                }
                else
                {
                    CurrentBehavior.Context.OrderId = null;
                }
            });

    public static LambdaStep LoadLastOrder(HttpClient client) =>
        new LambdaStep("Load last order")
            .HandleAsync(async () =>
            {
                var orderId = (string)CurrentBehavior.Context.OrderId;
                var url = $"api/order/{orderId}";
                var order = await client.GetFromJsonAsync<Order>(url);

                CurrentBehavior.Context.Order = order;
            });

    // ---

    public static LambdaStep CheckOrderId() =>
        new LambdaStep("Check order id is set")
            .Handle(() => { Assert.NotNull(CurrentBehavior.Context.OrderId); });

    public static LambdaStep CheckLastOrderNotEmpty() =>
        new LambdaStep("Check last order not empty")
            .Handle(() =>
            {
                var order = (Order)CurrentBehavior.Context.Order;
                Assert.NotEmpty(order.Items);
            });
}
