using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Models;
using System.Net;

namespace Store.Tests.Features;

public class LoginBehaviors : CommonSteps, IClassFixture<StoreApi>
{
    private readonly StoreApi _storeApi;

    public LoginBehaviors(StoreApi storeApi) => _storeApi = storeApi;

    [Behavior]
    [InlineData("jdoe", "password", HttpStatusCode.OK)]
    [InlineData("unknown", "bad password", HttpStatusCode.BadRequest)]
    public Behavior EnsurePasswordIsValidated(string user, string password, HttpStatusCode expected)
    {
        return new Behavior()
            .Given(ConfigureClient(_storeApi))
            .Given(ConfigureCredentials(user, password))
            .When<LoginStep>()
            .Then(CheckLastResponse(expected));
    }

    [Behavior]
    [InlineData("jdoe", "password")]
    public Behavior SuccessfulLoginReturnsToken(string user, string password)
    {
        return new Behavior()
            .Given(ConfigureClient(_storeApi))
            .Given(ConfigureCredentials(user, password))
            .When<LoginStep>()
            .Then(CheckValidToken());
    }

    public LambdaStep CheckValidToken() =>
        new LambdaThenStep()
            .Named("Check token is valid")
            .Handle<ApiContext>(context =>
                Assert.NotNull(context.Token));
}
