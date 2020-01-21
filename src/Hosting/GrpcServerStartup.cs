using Grpc.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlusUltra.AppMetrics.GrpcServer.Interpectors;
using PlusUltra.AppMetrics.GrpcServer.StandaloneMetricsServer;
using PlusUltra.GrpcWebServer.HostedServices;

namespace PlusUltra.GrpcWebServer.Hosting
{
    public abstract class GrpcServerStartup
    {
        protected readonly IConfiguration Configuration;
        protected readonly IWebHostEnvironment environment;
        public GrpcServerStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             services.AddStandaloneMetricsServer(options => {
                 options.ListenAnyIP(1010);
                 options.AllowSynchronousIO = true;
             });

            services.AddGrpc(options =>
            {
                //Primeiro parametro indica se devemos habilitar métricas de latenia
                options.Interceptors.Add<ServerMetricsInterceptor>();
            });
            services.AddHealthChecks();
            services.AddSingleton<HealthServiceImpl>();
            services.AddGrpcReflection();

            services.AddHostedService<StatusService>();
            
            AfterConfigureServices(services);
        }

        public abstract void AfterConfigureServices(IServiceCollection services);

        public abstract void BeforeConfigureApp(IApplicationBuilder app);

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILogger<GrpcServerStartup> logger)
        {
            BeforeConfigureApp(app);

            app.UseRouting();

            ConfigureAfterRouting(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<HealthServiceImpl>();
                endpoints.MapGrpcReflectionService();

                MapEndpoints(endpoints);
            });

            AfterConfigureApp(app);
        }

        public abstract void ConfigureAfterRouting(IApplicationBuilder app);

        public abstract void MapEndpoints(IEndpointRouteBuilder endpoints);

        public abstract void AfterConfigureApp(IApplicationBuilder app);
    }
}