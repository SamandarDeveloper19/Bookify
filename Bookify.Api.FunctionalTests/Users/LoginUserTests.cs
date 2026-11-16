using Bookify.Api.Controllers.Users;
using Bookify.Api.FunctionalTests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Bookify.Api.FunctionalTests.Users;

public class LoginUserTests : BaseFunctionalTest
{
    private const string Email = "login@test.com";
    private const string Password = "12345";

    public LoginUserTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Login_Should_ReturnUnauthorized_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new LogInUserRequest("missing@test.com", Password);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/user/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_Should_ReturnOk_WhenUserExists()
    {
        // Arrange
        var registerRequest = new RegisterUserRequest(Email, "first", "last", Password);
        await HttpClient.PostAsJsonAsync("api/v1/user/register", registerRequest);

        var request = new LogInUserRequest(Email, Password);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/user/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
