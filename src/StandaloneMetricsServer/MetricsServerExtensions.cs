using System;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace PlusUltra.GrpcWebServer.StandaloneMetricsServer
{
    public static class MetricsServerExtensions
    {
        public static IServiceCollection AddStandaloneMetricsServer(this IServiceCollection services,
                                                                 Action<KestrelServerOptions> configureServer = null)
        {
            services.AddHostedService<MetricsServerService>();

            services.Configure<MetricsServerOptions>(options =>
            {
                if (configureServer != null)
                {
                    options.ConfigureServerOptions += configureServer;
                }
            });
            return services;
        }
    }
}