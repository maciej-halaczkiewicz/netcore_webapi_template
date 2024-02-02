using template_app_application.Enums;
using template_app_infrastructure;
using template_app_infrastructure.Context;
using template_app_integration_tests.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace template_app_integration_tests.Base;

public class TestBase
{
    public const string AdminUser = "AdminUser";
    public const string SimpleUser = "SimpleUser";
    public const string SimpleUser2 = "SimpleUser2";
    private IConfigurationRoot configuration = null!;
    private UnitTestWebApplicationFactory app = null!;
    
    protected HttpClient HttpClient = null!;

    public async Task Initialize()
    {
        Environment.SetEnvironmentVariable("IS_INTEGRATION_TEST_ENVIRONMENT", "true");
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        configuration = configurationBuilder.Build();

        app = new UnitTestWebApplicationFactory(configuration);
        await app.InitializeAsync();
        HttpClient = app.CreateClient();

        var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TemplateAppContext>();
        await context.Database.MigrateAsync();
        await ResetState(scope);
    }

    public async Task StopAsync()
    {
        await app.StopAsync();
    }

    public async Task ResetState(IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<TemplateAppContext>();

        var seedData = new SeedDataHelper(context);
        await seedData.AddRole(Roles.User.ToString());
        await seedData.AddRole(Roles.Admin.ToString());

        await seedData.AddUser(AdminUser, AdminUser);
        await seedData.AddUser(SimpleUser, SimpleUser);
        await seedData.AddUser(SimpleUser2, SimpleUser2);

        await seedData.AddUserToRole(Roles.Admin.ToString(), AdminUser);
        await seedData.AddUserToRole(Roles.User.ToString(), AdminUser);
        await seedData.AddUserToRole(Roles.User.ToString(), SimpleUser);
        await seedData.AddUserToRole(Roles.User.ToString(), SimpleUser2);

    }
}