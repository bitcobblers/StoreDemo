using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Models;
using System.Net;

namespace Store.Tests.Features;

public class LoginBehaviors : IClassFixture<StoreApi>
{
    private readonly StoreApi _storeApi;

    public LoginBehaviors(StoreApi storeApi) => _storeApi = storeApi;

    [Behavior]
    [InlineData("jdoe", "password", HttpStatusCode.OK)]
    [InlineData("unknown", "bad password", HttpStatusCode.BadRequest)]
    public Behavior EnsurePasswordIsValidated(string user, string password, HttpStatusCode expected)
    {
        return new Behavior()
            .Given("New client", context => context.Client = _storeApi.CreateClient())
            .Given("Set credentials", context => context.Credentials = new LoginModel(user, password))
            .When<LoginStep>()
            .Then(CheckLastResponse(expected));
    }

    [Behavior]
    [InlineData("jdoe", "password")]
    public Behavior SuccessfulLoginReturnsToken(string user, string password)
    {
        return new Behavior()
            .Given("New client", context => context.Client = _storeApi.CreateClient())
            .Given("Set credentials", context => context.Credentials = new LoginModel(user, password))
            .When<LoginStep>()
            .Then(CheckValidToken());
    }

    public LambdaStep CheckLastResponse(HttpStatusCode expected) =>
        new LambdaThenStep()
            .Named("Check last response")
            .Handle(context =>
                Assert.Equal(expected, context.LastResponse.StatusCode));

    public LambdaStep CheckValidToken() =>
        new LambdaThenStep()
            .Named("Check token is valid")
            .Handle<ApiContext>(context =>
                Assert.NotNull(context.Token));
}
