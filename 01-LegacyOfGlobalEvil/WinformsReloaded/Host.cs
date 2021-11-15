using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WinformsReloaded.Abstractions;

namespace WinformsReloaded
{
    public class Host
    {
        private static CancellationTokenSource applicationExitTokenSource = new CancellationTokenSource();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static async Task Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var builder = new HostBuilder()
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config.AddJsonFile("appsettings.json", optional: true);
                   config.AddEnvironmentVariables();

                   if (args != null)
                   {
                       config.AddCommandLine(args);
                   }
               })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddOptions();
                   services.AddSingleton<IShell, Shell>();

                   // services.AddSIngleton<IHostedService, Host>();
                   services.AddSingleton<Host>();
               })
               .ConfigureLogging((hostingContext, logging) => {
                   logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                   logging.AddDebug();
               });

            // Can't do this if we want access to our service provider
            // await builder.RunConsoleAsync(applicationExitTokenSource.Token);

            // build the host - even tho' we're just using it as a wrapper
            var appHost = builder.Build();

            // Configure our servicelocator
            ServiceLocator.Initialize(appHost.Services);

            // start background services (IHostedService)
            await appHost.StartAsync();

            // this _has_ to run on the STAThread, so we can't use the normal appHost.RunAsync() method
            var clientHost = appHost.Services.GetRequiredService<Host>();
            clientHost.Run();

            // stop background services (IHostedService)
            await appHost.StopAsync();
        }

        private readonly IShell shell;
        private readonly ILogger logger;

        public Host(IShell shell, ILogger<Host> logger)
        {
            this.shell = shell ?? throw new ArgumentNullException(nameof(shell));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.shell.Closed += (s, e) =>
            {
                this.logger.LogInformation("Shutting down application - Shell closed");
                applicationExitTokenSource.Cancel();
            };
        }

        public void Run()
        {
            Application.Run(shell.MainForm);
        }
    }
}
