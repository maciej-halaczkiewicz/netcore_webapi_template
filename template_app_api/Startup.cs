using template_app_api.Implementations;
using template_app_application;
using template_app_application.Abstractions;
using template_app_infrastructure.Context;
using template_app_infrastructure.Repositories;
using template_app_infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Serilog;
using template_app_application.Enums;

namespace template_app_api
{
    public class Startup
    {
        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var IS_INTEGRATION_TEST_ENVIRONMENT = Environment.GetEnvironmentVariable("IS_INTEGRATION_TEST_ENVIRONMENT");

            // if not automatic tests, add negotation
            if (IS_INTEGRATION_TEST_ENVIRONMENT != "true")
            {
                services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
            }

            template_app_infrastructure.DependencyInjection.AddInfrastructure(services.AddApplication());

            var connectionString = Configuration.GetConnectionString("default");
            services.AddDbContext<TemplateAppContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton(Log.Logger);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClaimsTransformation, MyClaimsTransformation>();
            services.AddScoped<IUserContext, UserContext>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(template_app_infrastructure.DependencyInjection).Assembly);
            });
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireClaim("Role", Roles.Admin.ToString()));
                options.AddPolicy("UserPolicy", policy =>
                    policy.RequireClaim("Role", Roles.User.ToString()));
            });
        }

        public void Configure(WebApplication app)
        {
            app.UseSerilogRequestLogging();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            var IS_INTEGRATION_TEST_ENVIRONMENT = Environment.GetEnvironmentVariable("IS_INTEGRATION_TEST_ENVIRONMENT");

            if (IS_INTEGRATION_TEST_ENVIRONMENT != "true")
            {
                app.UseDefaultFiles(new DefaultFilesOptions
                {
                    DefaultFileNames = new List<string> { "index.html" }
                });
                app.UseStaticFiles();
            }

            app.MapControllers();
        }
    }
}