using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Grpc.Health.V1;
using Grpc.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;

namespace PlusUltra.Grpc.WebServer.HostedServices
{
    public class StatusService : BackgroundService
    {
        private readonly HealthServiceImpl _healthService;
        private readonly HealthCheckService _healthCheckService;

        public StatusService(HealthServiceImpl healthService, HealthCheckService healthCheckService)
        {
            _healthService = healthService;
            _healthCheckService = healthCheckService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var health = await _healthCheckService.CheckHealthAsync(stoppingToken);

                _healthService.SetStatus("Service",
                    health.Status == HealthStatus.Healthy
                        ? HealthCheckResponse.Types.ServingStatus.Serving
                        : HealthCheckResponse.Types.ServingStatus.NotServing);

                _healthService.SetStatus(string.Empty,
                    health.Status == HealthStatus.Healthy
                        ? HealthCheckResponse.Types.ServingStatus.Serving
                        : HealthCheckResponse.Types.ServingStatus.NotServing);

                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }
    }
}