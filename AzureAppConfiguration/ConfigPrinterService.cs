using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureAppConfiguration
{
    public class ConfigPrinterService<T> : IHostedService
    {
        private readonly IOptionsMonitor<T> _configMonitor;
        private readonly ILogger _logger;
        private readonly Timer _timer;

        public ConfigPrinterService(IOptionsMonitor<T> monitor, ILogger<ConfigPrinterService<T>> logger)
        {
            _configMonitor = monitor ?? throw new ArgumentNullException(nameof(monitor));
            logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _timer = new Timer(DumpConfig);

            monitor.OnChange(config =>
            {
                _logger.LogCritical("Config changed!");
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // start the timer
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DumpConfig(object state)
        {
            Console.WriteLine($"Configuration: {_configMonitor.CurrentValue}");
        }
    }
}
