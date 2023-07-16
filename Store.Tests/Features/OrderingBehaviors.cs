using DrillSergeant;
using DrillSergeant.GWT;
using Store.Tests.Steps;

namespace Store.Tests.Features;

public class OrderingBehaviors : IClassFixture<StoreApi>
{
    private readonly StoreApi _api;

    public OrderingBehaviors(StoreApi api) => _api = api;

    [Behavior]
    public void PurchasingItemsInCartCreatesNewOrder()
    {
        var client = _api.CreateClient();

        BehaviorBuilder.New()
            .Given(CartSteps.NewCart(client))
            .Given(CartSteps.LoadProducts(client))
            .Given(CartSteps.AddRandomProductToCart(client))
            .When(OrderingSteps.PlaceOrder(client))
            .Then(OrderingSteps.CheckOrderId());
    }

    [Behavior]
    public void LoadingLastOrderNumberReturnsListOfItems()
    {
        var client = _api.CreateClient();

        BehaviorBuilder.New()
            .Given(CartSteps.NewCart(client))
            .Given(CartSteps.LoadProducts(client))
            .Given(CartSteps.AddRandomProductToCart(client))
            .When(OrderingSteps.PlaceOrder(client))
            .When(OrderingSteps.LoadLastOrder(client))
            .Then(OrderingSteps.CheckLastOrderNotEmpty());
    }
}
