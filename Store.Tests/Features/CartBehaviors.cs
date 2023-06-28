using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Database;
using System.Net.Http.Json;

namespace Store.Tests.Features;

public class CartBehaviors : CommonSteps, IClassFixture<StoreApi>
{
    protected readonly StoreApi _storeApi;

    protected CartBehaviors(StoreApi storeApi) => _storeApi = storeApi;

    public class ClearBehaviors : CartBehaviors, IClassFixture<StoreApi>
    {
        public ClearBehaviors(StoreApi storeApi)
            : base(storeApi)
        {
        }

        [Behavior]
        public Behavior ClearingUnknownCartReturnsNotFound()
        {
            return new Behavior()
                .Given(ConfigureClient(_storeApi))
                .When(ClearCartStep(-1))
                .Then(CheckLastResponse(System.Net.HttpStatusCode.NotFound));
        }

        [Behavior]
        public Behavior ClearingKnownCartReturnsOk()
        {
            return new Behavior()
                .Given(ConfigureClient(_storeApi))
                .GivenAsync("Create the cart", async context =>
                {
                    context.CartId = await NewCart(context.Client);
                })
                .WhenAsync("Clear the cart", async context =>
                {
                    context.LastResponse = await ClearCart(context.Client, context.CartId);
                })
                .Then(CheckLastResponse(System.Net.HttpStatusCode.OK));
        }
    }

    public class LoadBehaviors : CartBehaviors, IClassFixture<StoreApi>
    {
        public LoadBehaviors(StoreApi storeApi)
            : base(storeApi)
        {
        }

        [Behavior]
        public Behavior LoadingUnknownCartSetsCartToNullInContext()
        {
            return new Behavior()
                .Given(ConfigureClient(_storeApi))
                .Given("Set dummy cart id", context => context.CartId = -1)
                .When(LoadCartStep())
                .Then("Cart should not be defined", context =>
                {
                    Assert.Null(context.Cart);
                });
        }

        [Behavior]
        public Behavior LoadingKnownCartSetsCartInContext()
        {
            return new Behavior()
                .Given(ConfigureClient(_storeApi))
                .When(NewCartStep())
                .When(LoadCartStep())
                .Then("Cart should not be defined", context =>
                {
                    Assert.NotNull(context.Cart);
                });
        }
    }

    public class NewBehaviors : CartBehaviors, IClassFixture<StoreApi>
    {
        public NewBehaviors(StoreApi storeApi)
            : base(storeApi)
        {
        }

        [Behavior]
        public Behavior CreatesNewIdWithEachCall()
        {
            return new Behavior()
                .Given(ConfigureClient(_storeApi))
                .GivenAsync("Create a new cart", async context =>
                {
                    context.Cart1 = await NewCart(context.Client);
                })
                .GivenAsync("Create a second new cart", async context =>
                {
                    context.Cart2 = await NewCart(context.Client);
                })
                .Then("Check cart ids", context =>
                {
                    Assert.NotEqual(context.Cart1, context.Cart2);
                });
        }
    }

    public LambdaStep ClearCartStep(int id) =>
        new LambdaWhenStep()
            .Named($"Clear cart: {id}")
            .HandleAsync(async context =>
            {
                context.LastResponse = await ClearCart(context.Client, id);
            });

    public LambdaStep LoadCartStep() =>
        new LambdaWhenStep()
            .Named($"Load cart")
            .HandleAsync(async context =>
            {
                var client = (HttpClient)context.Client;
                var id = (int)context.CartId;
                var url = $"api/cart/load/{id}";
                
                try
                {
                    context.Cart = await client.GetFromJsonAsync<ShoppingCart>(url);
                }
                catch(HttpRequestException)
                {
                    context.Cart = null;
                }
            });

    public LambdaStep NewCartStep() =>
        new LambdaGivenStep()
            .Named("Create cart")
            .HandleAsync(async context =>
            {
                context.CartId = await NewCart(context.Client);
            });

    protected async Task<int> NewCart(HttpClient client)
    {
        var url = $"api/cart/new";
        var response = await client.GetStringAsync(url);

        return int.Parse(response);
    }

    protected Task<HttpResponseMessage> ClearCart(HttpClient client, int id)
    {
        var url = $"api/cart/clear";
        return client.PostAsJsonAsync(url, id);
    }
}
