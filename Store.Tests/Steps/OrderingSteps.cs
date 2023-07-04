using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Database;
using Store.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace Store.Tests.Steps;

public static class OrderingSteps
{
    public static LambdaStep PlaceOrder(HttpClient client) =>
        new LambdaStep("Place order")
            .HandleAsync(async context =>
            {
                var cartId = (int)context.CartId;
                var order = new PlaceOrderRequest(cartId);
                var url = "api/order/place";

                var response = await client.PostAsJsonAsync(url, order);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var body = await response
                        .Content
                        .ReadFromJsonAsync<PlaceOrderResponse>();

                    context.OrderId = body?.OrderNumber;
                }
                else
                {
                    context.OrderId = null;
                }
            });

    public static LambdaStep LoadLastOrder(HttpClient client) =>
        new LambdaStep("Load last order")
            .HandleAsync(async context =>
            {
                var orderId = (string)context.OrderId;
                var url = $"api/order/{orderId}";
                var order = await client.GetFromJsonAsync<Order>(url);

                context.Order = order;
            });

    // ---

    public static LambdaStep CheckOrderId() =>
        new LambdaStep("Check order id is set")
            .Handle(context =>
            {
                Assert.NotNull(context.OrderId);
            });

    public static LambdaStep CheckLastOrderNotEmpty() =>
        new LambdaStep("Check last order not empty")
            .Handle(context =>
            {
                var order = (Order)context.Order;
                Assert.NotEmpty(order.Items);
            });
}
