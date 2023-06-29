using DrillSergeant;
using DrillSergeant.GWT;
using Store.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace Store.Tests.Steps;

public static class LoginSteps
{
    public static LambdaStep Login(HttpClient client, string user, string password) =>
        new WhenLambdaStep()
            .Named("Login")
            .HandleAsync(async context =>
            {
                var credentials = new LoginRequest(user, password);
                var response = await client.PostAsJsonAsync("api/login", credentials);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var body = await response
                        .Content
                        .ReadFromJsonAsync<LoginResponse>();

                    context.Token = body?.Token;
                }
                else
                {
                    context.Token = null;
                }

                context.LastResponse = response;
            });

    public static LambdaStep CheckToken(bool isValid) =>
        new ThenLambdaStep()
            .Named("Check token is valid")
            .Handle(context =>
            {
                if (isValid)
                {
                    Assert.NotNull(context.Token);
                }
                else
                {
                    Assert.Null(context.Token);
                }
            });
}