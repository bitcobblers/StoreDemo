using DrillSergeant;
using DrillSergeant.GWT;
using Store.Tests.Steps;

namespace Store.Tests.Features;

public class LoginBehaviors : IClassFixture<StoreApi>
{
    private readonly StoreApi _storeApi;

    public LoginBehaviors(StoreApi storeApi) => _storeApi = storeApi;

    [Behavior]
    [InlineData("jdoe", "password", true)]
    [InlineData("unknown", "bad password", false)]
    public Behavior EnsurePasswordIsValidated(string user, string password, bool expected)
    {
        var client = _storeApi.CreateClient();

        return new Behavior()
            .When(LoginSteps.Login(client, user, password))
            .Then(LoginSteps.CheckToken(expected));
    }
}
