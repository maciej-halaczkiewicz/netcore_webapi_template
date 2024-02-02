using FluentAssertions;
using NUnit.Framework;
using System.Net.Http.Json;
using template_app_application.Enums;
using template_app_application.Users;
using template_app_integration_tests.Authentication;
using template_app_integration_tests.Base;

namespace template_app_integration_tests.Tests.WebApi;
public class GetCurrentUserInfoIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ThatAdminCanFetchInfoAboutHim()
    {
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add(TestAuthHandler.UserId, TestBase.AdminUser);
        var result = await HttpClient.GetFromJsonAsync<CurrentUserInfoDto>("/api/current-user-info");

        result.Should().NotBeNull();
        result.Username.Should().Be(TestBase.AdminUser);
        result.Roles.Should().Contain(x => x.RoleName == Roles.Admin.ToString());
    }

    [Test]
    public async Task ThatUserCanFetchInfoAboutHim()
    {
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add(TestAuthHandler.UserId, TestBase.SimpleUser);

        var result = await HttpClient.GetFromJsonAsync<CurrentUserInfoDto>("/api/current-user-info");
        result.Should().NotBeNull();
        result.Username.Should().Be(TestBase.SimpleUser);
        result.Roles.Should().Contain(x => x.RoleName == Roles.User.ToString());
    }
}