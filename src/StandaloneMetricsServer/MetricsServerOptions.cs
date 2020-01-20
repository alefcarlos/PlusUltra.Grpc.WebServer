using System;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace  PlusUltra.GrpcWebServer.StandaloneMetricsServer
{
    public class MetricsServerOptions
    {
        public Action<KestrelServerOptions> ConfigureServerOptions { get; set; } = o => { };
    }
}