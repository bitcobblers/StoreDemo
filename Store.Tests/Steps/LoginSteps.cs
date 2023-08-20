using DrillSergeant;
using Store.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace Store.Tests.Steps;

public static class LoginSteps
{
    public static LambdaStep Login(HttpClient client, string user, string password) =>
        new LambdaStep("Login")
            .HandleAsync(async () =>
            {
                var credentials = new LoginRequest(user, password);
                var response = await client.PostAsJsonAsync("api/login", credentials);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var body = await response
                        .Content
                        .ReadFromJsonAsync<LoginResponse>();

                    CurrentBehavior.Context.Token = body?.Token;
                }
                else
                {
                    CurrentBehavior.Context.Token = null;
                }

                CurrentBehavior.Context.LastResponse = response;
            });

    public static LambdaStep CheckToken(bool isValid) =>
        new LambdaStep("Check token is valid")
            .Handle(() =>
            {
                if (isValid)
                {
                    Assert.NotNull(CurrentBehavior.Context.Token);
                }
                else
                {
                    Assert.Null(CurrentBehavior.Context.Token);
                }
            });
}