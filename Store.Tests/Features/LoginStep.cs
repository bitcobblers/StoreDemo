using DrillSergeant.GWT;
using Store.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace Store.Tests.Features;

public class LoginStep : WhenStep
{
    public async Task When(ApiContext context)
    {
        var response  = await context.Client.PostAsJsonAsync("api/Login", context.Credentials, CancellationToken.None);

        if(response.StatusCode == HttpStatusCode.OK)
        {
            var body = await response
                .Content
                .ReadFromJsonAsync<LoginResponse>();

            context.Token = body?.Token;
        }

        context.LastResponse = response;
    }
}

