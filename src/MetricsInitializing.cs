using App.Metrics;

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