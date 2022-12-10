using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework;
using Spirebyte.Framework.Auth;
using Spirebyte.Services.Git.Application;
using Spirebyte.Services.Git.Core.Constants;
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

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) => services
                .AddApplication()
                .AddInfrastructure(ctx.Configuration)
                .Configure<AuthorizationOptions>(options =>
                {
                    options.AddEitherOrScopePolicy(ApiScopes.RepositoriesRead, ApiScopes.RepositoriesRead, ApiScopes.RepositoriesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.RepositoriesWrite, ApiScopes.RepositoriesWrite, ApiScopes.RepositoriesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.RepositoriesDelete, ApiScopes.RepositoriesDelete, ApiScopes.RepositoriesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.RepositoriesCommit, ApiScopes.RepositoriesCommit, ApiScopes.RepositoriesManage);
                })
                .AddControllers()
            )
            .Configure(app => app
                .UseSpirebyteFramework()
                .UseApplication()
                .UseInfrastructure()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("",
                            ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppInfo>().Name));
                        endpoints.MapGet("/ping", () => "pong");
                        endpoints.MapControllers();
                    }
                ))
            .AddSpirebyteFramework();
    }
}