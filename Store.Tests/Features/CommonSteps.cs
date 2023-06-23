using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Models;

namespace Store.Tests.Features;

public class CommonSteps
{
    public LambdaStep ConfigureClient(StoreApi storeApi) =>
        new LambdaGivenStep()
            .Named("Setup client")
            .Handle<ApiContext>(context => context.Client = storeApi.CreateClient());

    public LambdaStep ConfigureCredentials(string user, string password) =>
        new LambdaGivenStep()
            .Named("Configure credentials")
            .Handle<ApiContext>(context => context.Credentials = new LoginRequest(user, password));
}
