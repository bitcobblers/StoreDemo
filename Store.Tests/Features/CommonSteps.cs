using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Models;
using System.Net;

namespace Store.Tests.Features;

public class CommonSteps
{

    public LambdaStep CheckLastResponse(HttpStatusCode expected) =>
        new LambdaThenStep()
            .Named("Check last response")
            .Handle<ApiContext>(context =>
                Assert.Equal(expected, context.LastResponse?.StatusCode));
    public LambdaStep ConfigureClient(StoreApi storeApi) =>
        new LambdaGivenStep()
            .Named("Setup client")
            .Handle<ApiContext>(context => context.Client = storeApi.CreateClient());

    public LambdaStep ConfigureCredentials(string user, string password) =>
        new LambdaGivenStep()
            .Named("Configure credentials")
            .Handle<ApiContext>(context => context.Credentials = new LoginRequest(user, password));
}
