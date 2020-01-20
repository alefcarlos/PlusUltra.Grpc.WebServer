using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlusUltra.GrpcWebServer.Hosting;

namespace GrpcSample
{
    public class Startup : GrpcServerStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
            : base(configuration, environment)
        {
        }

        public override void AfterConfigureApp(IApplicationBuilder app)
        {
        }

        public override void AfterConfigureServices(IServiceCollection services)
        {
        }

        public override void BeforeConfigureApp(IApplicationBuilder app)
        {
        }

        public override void ConfigureAfterRouting(IApplicationBuilder app)
        {
        }

        public override void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGrpcService<GreeterService>();
        }
    }
}
