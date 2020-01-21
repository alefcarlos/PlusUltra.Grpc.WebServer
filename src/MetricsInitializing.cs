using App.Metrics;
using App.Metrics.Counter;

namespace PlusUltra.GrpcWebServer
{
    public static class MetricsInitializing
    {
        public static IMetricsRoot Metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Build();
    }
}