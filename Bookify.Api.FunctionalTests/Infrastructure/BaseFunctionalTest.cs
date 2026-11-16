using Bookify.Api.Controllers.Users;
using Bookify.Api.FunctionalTests.Users;
using Bookify.Application.Users.LoginUser;
using System.Net.Http.Json;

namespace Bookify.Api.FunctionalTests.Infrastructure;

[Collection(FunctionalTestCollection.Name)]
public abstract class BaseFunctionalTest
{
    protected readonly HttpClient HttpClient;

    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }

    protected async Task<string> GetAccessToken()
    {
        HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync(
            "api/v1/user/login",
            new LogInUserRequest(
                UserData.RegisterTestUserRequest.Email,
                UserData.RegisterTestUserRequest.Password));

        loginResponse.EnsureSuccessStatusCode();

        var accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();

        return accessTokenResponse?.AccessToken
            ?? throw new InvalidOperationException("Login succeeded but returned no access token.");
    }
}
