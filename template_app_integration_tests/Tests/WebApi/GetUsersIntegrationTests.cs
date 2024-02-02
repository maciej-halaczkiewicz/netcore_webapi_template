using System.Net;
using System.Net.Http.Json;
using template_app_application.Helpers;
using template_app_application.Users;
using template_app_integration_tests.Authentication;
using template_app_integration_tests.Base;
using FluentAssertions;
using NUnit.Framework;

namespace template_app_integration_tests.Tests.WebApi;
public class GetUsersIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ThatAdminCanFetchUserList()
    {
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add(TestAuthHandler.UserId, TestBase.AdminUser);
        var users = await HttpClient.GetFromJsonAsync<PageableList<UserDto>>("/api/user-list");

        users.Should().NotBeNull();
    }

    [Test]
    public async Task ThatUserCanNotFetchUserList()
    {
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add(TestAuthHandler.UserId, TestBase.SimpleUser);
        var response = await HttpClient.GetAsync("/api/user-list");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}