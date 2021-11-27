// See https://aka.ms/new-console-template for more information

using AzureAppConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new HostBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: true);
            config.AddEnvironmentVariables();
            config.AddUserSecrets("7d524cf2-3bc4-4016-b4b3-878f55592b4a");

            var settings = config.Build();

            config.AddAzureAppConfiguration(options =>
            {
                options.Connect(settings["AzureAppConfig:ConnectionString"]);
                options.ConfigureRefresh(refresh =>
                {
                    refresh.Register("AppConfig.AppVersion", refreshAll:true)
                        .SetCacheExpiration(TimeSpan.FromSeconds(7));
                });
            });
        })
    .ConfigureServices((hostContext, services) =>
        {
            services.AddOptions();
            services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));

            services.AddSingleton<IHostedService, ConfigPrinterService<AppConfig>>();
        })
    .ConfigureLogging((hostContext, logging) =>
    {
        logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
        logging.AddConsole();
    });

await builder.RunConsoleAsync();