using Serilog;
using Serilog.Events;

namespace template_app_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("APP_BASE_DIRECTORY", AppContext.BaseDirectory);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .CreateLogger();

            try
            {
                Log.Information("Starting host");
                StartApplication(args);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        private static void StartApplication(string[] args)
        {
            var hostBuilder = WebApplication.CreateBuilder(args);
            var startup = new Startup(hostBuilder.Configuration);
            startup.ConfigureServices(hostBuilder.Services);
            
            hostBuilder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration));
            
            var app = hostBuilder.Build();
            startup.Configure(app);
            app.Run();
        }
    }
}
