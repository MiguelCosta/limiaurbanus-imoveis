namespace Mpc.LimiaUrbanus.DataBase.RunExample
{
    using System;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Mpc.LimiaUrbanus.DataBase.Models;

    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static ILogger<Program> _logger;

        private static void Main(string[] args)
        {
            var services = new ServiceCollection();
            Configure();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            ConfigureLogging(serviceProvider);

            _logger.LogInformation("START");
            var info = serviceProvider.GetService<IInformationService>();
            info.GetDistritos();

            Console.ReadLine();
        }

        #region Configuration

        private static void Configure()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true);

            _configuration = builder.Build();
        }

        private static void ConfigureLogging(IServiceProvider serviceProvider)
        {
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            _logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<IInformationService, InformationService>();
            services.AddDbContext<LimiaUrbanusContext>(options =>
                    options.UseSqlServer(_configuration.GetConnectionString("LimiaUrbanusDatabase")));
        }

        #endregion Configuration
    }
}
