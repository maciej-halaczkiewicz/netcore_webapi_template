using template_app_api;
using template_app_application.Abstractions;
using template_app_integration_tests.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Testcontainers.MsSql;

namespace template_app_integration_tests.Implementations;

public class UnitTestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IConfigurationRoot _configuration;
    private readonly MsSqlContainer _dbContainer;
    private readonly int _port = Random.Shared.Next(10000, 15000);
    public UnitTestWebApplicationFactory(IConfigurationRoot configuration)
    {
        _configuration = configuration;
        _dbContainer = new MsSqlBuilder()
            .WithPortBinding(_port,
                1433)
            .WithName("mssql_" + _port)
            .Build();
    }
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task StopAsync()
    {
        await _dbContainer.StopAsync();
    }

    private void GetServices(IServiceCollection services)
    {
        var startup = new Startup(_configuration);

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "template_app_api"));

        services.AddLogging();

        // should be used for unit tests, not integration tests:
        //services.AddScoped<IUserContext, MockedUserContext>();

        services.Configure<TestAuthHandlerOptions>(options =>
        {
            options.DefaultUserName = "john.doe";
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

        startup.ConfigureServices(services);


        // Replace service registration for ICurrentUserService
        // Remove existing registration
        //var currentUserServiceDescriptor = services.FirstOrDefault(d =>
        //    d.ServiceType == typeof(ICurrentUserService));

        //if (currentUserServiceDescriptor != null)
        //{
        //    services.Remove(currentUserServiceDescriptor);
        //}

        // Register testing version
        //services.AddTransient(provider => Mock.Of<ICurrentUserService>(s => s.UserId == s_currentUserId));
    }
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((ctx, conf) =>
        {
            conf.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables(); ;
        });

        builder.ConfigureHostConfiguration(x =>
        {
            x.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
            {
                new("ConnectionStrings:default",
                    _dbContainer.GetConnectionString()
                        .Replace("master", "TemplateApp")),
            });
        });

        builder.ConfigureServices(GetServices);
        return base.CreateHost(builder);
    }
}