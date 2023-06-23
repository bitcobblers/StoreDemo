using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Database;
using System.Net.Http.Json;

namespace Store.Tests.Features;

public class ProductsBehaviors : CommonSteps, IClassFixture<StoreApi>
{
    private readonly StoreApi _storeApi;

    public ProductsBehaviors(StoreApi storeApi) => _storeApi = storeApi;

    [Behavior]
    public Behavior EnsureProductListIsNotEmpty()
    {
        return new Behavior()
            .Given(ConfigureClient(_storeApi))
            .When(ListProducts())
            .Then(CheckForProducts());
    }

    public LambdaStep ListProducts() =>
        new LambdaWhenStep()
            .Named("Get product list")
            .HandleAsync(async context =>
            {
                var client = (HttpClient)context.Client;
                var response = await client.GetFromJsonAsync<Product[]>("api/products", CancellationToken.None);

                context.Products = response;
            });

    public LambdaStep CheckForProducts() =>
        new LambdaThenStep()
            .Named("Check for products")
            .Handle(context =>
            {
                Assert.NotNull(context.Products);
                Assert.NotEmpty(context.Products);
            });
}
