namespace Mpc.LimiaUrbanus.DataBase.RunExample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Mpc.LimiaUrbanus.DataBase.Models;
    using Mpc.LimiaUrbanus.Services;

    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static ILogger<Program> _logger;

        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            Configure();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            ConfigureLogging(serviceProvider);

            _logger.LogInformation("START");

            var imoveis2Export = new List<string>
            {
                "mds2337",
                "lu2028",
                "lu1128",
                "mds2697",
                "lu1115",
                "MDS2139",
                "lu1114",
                "mds0517",
                "cas_348",
                "lu1126",
                "lu0040",
                "lu0042",
                "lu1116",
                "mds0320",
                "lu1117",
                "150194",
                "140032"
            };

            var info = serviceProvider.GetService<IInformationService>();
            var xml = await info.GetXmlFromImoveisAsync(imoveis2Export);
            await info.SaveXmlTextToFileAsync(xml);

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
            services.AddSingleton(_configuration);
            services.AddSingleton<IInformationService, InformationService>();
            services.AddSingleton<IImovirtualXmlGenerator, ImovirtualXmlGenerator>();
            services.AddDbContext<LimiaUrbanusContext>(options =>
                    options.UseSqlServer(_configuration.GetConnectionString("LimiaUrbanusDatabase")));
        }

        #endregion Configuration
    }
}
