using DrillSergeant;
using static DrillSergeant.GWT;
using Store.Tests.Steps;

namespace Store.Tests.Features;

public class ShoppingCartBehaviors : IClassFixture<StoreApi>
{
    private readonly StoreApi _api;

    public ShoppingCartBehaviors(StoreApi api) => _api = api;

    [Behavior]
    public void CreatingNewCartReturnsEmptyCart()
    {
        var client = _api.CreateClient();

        Given(CartSteps.NewCart(client));
        When(CartSteps.LoadCurrentCart(client));
        Then(CartSteps.CheckCartSize(0));
    }

    [Behavior]
    public void AddingItemsToCartUpdatesCart()
    {
        var client = _api.CreateClient();

        Given(CartSteps.NewCart(client));
        Given(CartSteps.LoadProducts(client));
        When(CartSteps.AddRandomProductToCart(client));
        When(CartSteps.LoadCurrentCart(client));
        Then(CartSteps.CheckCartSize(1));
    }

    [Behavior]
    public void ClearingCartRemovesAllItemsFromIt()
    {
        var client = _api.CreateClient();

        Given(CartSteps.NewCart(client));
        Given(CartSteps.LoadProducts(client));
        When(CartSteps.AddRandomProductToCart(client));
        When(CartSteps.ClearCurrentCart(client));
        When(CartSteps.LoadCurrentCart(client));
        Then(CartSteps.CheckCartSize(0));
    }
}