using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UserApi;

namespace Lab2.Tests;

public class UserApiTests : IDisposable
{
    private const string Hash = "ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f";

    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"lab2-users-{Guid.NewGuid():N}.db");
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UserApiTests()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(b =>
            b.UseSetting("ConnectionStrings:Users", $"Data Source={_dbPath};Pooling=False"));
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _factory.Dispose();
        File.Delete(_dbPath);
    }

    private Task<HttpResponseMessage> Post(string? login, string? passHash) =>
        _client.PostAsJsonAsync("/user", new UserRequest(login, passHash));

    [Fact]
    public async Task Crud_FullCycle()
    {
        var created = await Post("example_user", Hash);
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        var user = await created.Content.ReadFromJsonAsync<UserResponse>();
        Assert.Equal("example_user", user!.Login);

        var read = await _client.GetFromJsonAsync<UserResponse>($"/user/{user.Id}");
        Assert.Equal(user, read);

        var updated = await _client.PutAsJsonAsync($"/user/{user.Id}", new UserRequest("renamed_user", Hash));
        Assert.Equal("renamed_user", (await updated.Content.ReadFromJsonAsync<UserResponse>())!.Login);

        Assert.Equal(HttpStatusCode.OK, (await _client.DeleteAsync($"/user/{user.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync($"/user/{user.Id}")).StatusCode);
    }

    [Fact]
    public async Task Post_DuplicateLogin_ReturnsConflict()
    {
        await Post("example_user", Hash);

        Assert.Equal(HttpStatusCode.Conflict, (await Post("example_user", Hash)).StatusCode);
    }

    [Theory]
    [InlineData("bob", "qwerty")]
    [InlineData("bob", null)]
    [InlineData(null, Hash)]
    [InlineData("b", Hash)]
    public async Task Post_InvalidData_ReturnsBadRequestWithJsonMessage(string? login, string? passHash)
    {
        var response = await Post(login, passHash);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.False(string.IsNullOrEmpty((await response.Content.ReadFromJsonAsync<MessageResponse>())!.Message));
    }

    [Fact]
    public async Task Get_DoesNotExposePassHash()
    {
        await Post("example_user", Hash);

        var json = await _client.GetStringAsync("/user/1");

        Assert.DoesNotContain("PassHash", json);
        Assert.DoesNotContain(Hash, json);
    }

    [Fact]
    public async Task Put_ToLoginOfAnotherUser_ReturnsConflict()
    {
        await Post("first_user", Hash);
        await Post("second_user", Hash);

        var response = await _client.PutAsJsonAsync("/user/2", new UserRequest("first_user", Hash));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task UnknownId_ReturnsNotFound()
    {
        Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync("/user/99")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _client.DeleteAsync("/user/99")).StatusCode);
    }
}
