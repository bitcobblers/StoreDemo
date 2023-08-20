using DrillSergeant;
using static DrillSergeant.GWT;
using Store.Tests.Steps;

namespace Store.Tests.Features;

public class LoginBehaviors : IClassFixture<StoreApi>
{
    private readonly StoreApi _storeApi;

    public LoginBehaviors(StoreApi storeApi) => _storeApi = storeApi;

    [Behavior]
    [InlineData("jdoe", "password", true)]
    [InlineData("unknown", "bad password", false)]
    public void EnsurePasswordIsValidated(string user, string password, bool expected)
    {
        var client = _storeApi.CreateClient();

        When(LoginSteps.Login(client, user, password));
        Then(LoginSteps.CheckToken(expected));
    }
}
