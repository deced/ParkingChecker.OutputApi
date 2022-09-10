using System.Reflection;
using ParkingChecker.OutputApi.Base.DataAccess;
using ParkingChecker.OutputApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ParkingChecker.OutputApi.Base.Commands;
using MongoDBMigrations;
using ParkingChecker.OutputApi.Configuration;

namespace Lexily.ManagementWarehouseService.Configuration
{
    public static class DependencyStartup
    {
        private static void ConfigureMigrations(IConfiguration configuration)
        {
            var databaseConfiguration =
                configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

            new MigrationEngine()
                .UseDatabase(databaseConfiguration.ConnectionString,
                    databaseConfiguration.DatabaseName)
                .UseAssembly(Assembly.GetExecutingAssembly())
                .UseSchemeValidation(false).Run();
        }

        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            AddInfrastructure(services);
            AddCommands(services);
            ConfigureDatabase(services, configuration);
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfiguration>(configuration.GetSection("DatabaseConfiguration"));
            services.AddSingleton<IDatabaseConfiguration>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
        }

        private static void AddCommands(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseGetAllCommand<>), typeof(BaseGetAllCommand<>));
            services.AddScoped(typeof(IBaseGetByIdCommand<>), typeof(BaseGetByIdCommand<>));
            services.AddScoped(typeof(IBaseFilterCommand<>), typeof(BaseFilterCommand<>));
            services.AddScoped(typeof(IBaseCountCommand<>), typeof(BaseCountCommand<>));
            services.AddScoped(typeof(IBaseFindOneCommand<>), typeof(BaseFindOneCommand<>));
            
            //services.AddScoped<IUpdateCountryWithLocalizationsAndCostRange,UpdateCountryWithLocalizationsAndCostRange>();
        }

        private static void AddInfrastructure(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IParkingService, ParkingService>();
            services.AddScoped<IParkingSpotService, ParkingSpotService>();
        }
    }
}