using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace PlusUltra.GrpcWebServer.HostedServices
{
    public class MetricsServerService : BackgroundService
    {
        public MetricsServerService(ILogger<MetricsServerService> logger)
        {
            this.logger = logger;
        }

        private readonly ILogger logger;
        
        private MetricServer _metricServer;
        private IDisposable diagnosticSourceRegistration; 

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting Prometheus HTTTP/1 MetricServer on port 1010");

            _metricServer = new MetricServer(port: 1010);
            _metricServer.Start();

            diagnosticSourceRegistration = DiagnosticSourceAdapter.StartListening();

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)  {
            logger.LogInformation("Prometheus MetricServer Application is shutting down...");
            await _metricServer.StopAsync();
            diagnosticSourceRegistration.Dispose();
        }
    }
}