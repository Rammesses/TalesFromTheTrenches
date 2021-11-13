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
    public class Host : IHostedService
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
                   services.AddSingleton<IHostedService, Host>();
               })
               .ConfigureLogging((hostingContext, logging) => {
                   logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                   logging.AddDebug();
               });

            await builder.RunConsoleAsync(applicationExitTokenSource.Token);
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

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Starting shell...");
            Task.Run(() => Application.Run(shell.MainForm));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Shutting down application (CTRL-C or SIGTERM)");
            Application.Exit();
            return Task.CompletedTask;
        }
    }
}
