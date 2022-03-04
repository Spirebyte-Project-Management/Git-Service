using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Git.Application;
using Spirebyte.Services.Git.Infrastructure;

namespace Spirebyte.Services.Git.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateWebHostBuilder(args)
            .Build()
            .RunAsync();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddControllers();
                services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build();
            })
            .Configure(app => app
                .UseInfrastructure()
                .UseRouting()
                .UseAuthorization()
                .UsePingEndpoint()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("",
                            ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>()?.Name!));
                        endpoints.MapControllers();
                    }
                ))
            .UseLogging()
            .UseVault();
    }
}