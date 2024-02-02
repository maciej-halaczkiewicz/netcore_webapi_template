using template_app_infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace template_app_data_migrations
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilder, services) =>
                {
                    services.AddTransient<SeedDataHelper>();
                    var connectionString = hostBuilder.Configuration.GetConnectionString("default");
                    services.AddDbContext<TemplateAppContext>(options => options.UseSqlServer(connectionString));
                })
                .Build();

            var context = host.Services.GetRequiredService<TemplateAppContext>();
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await context.Database.EnsureCreatedAsync();

                var clearScript = @"DELETE ""dbo"".""UserRole"";
                             DELETE ""dbo"".""User"";
                             DELETE ""dbo"".""Role"";
                             DELETE ""dbo"".""AuditHistory"";";
                await context.Database.ExecuteSqlRawAsync(clearScript);

                var seedData = host.Services.GetRequiredService<SeedDataHelper>();
                await seedData.AddRole("User");
                await seedData.AddRole("Admin");

                await seedData.AddUser("User1");
                await seedData.AddUser("User2");
                await seedData.AddUser("User3");

                await seedData.AddUserToRole("User", "MACIEK_PC\\macie");
                await seedData.AddUserToRole("Admin", "MACIEK_PC\\macie");
                await seedData.AddUserToRole("User", "User3");
                await seedData.AddUserToRole("User", "User2");
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
