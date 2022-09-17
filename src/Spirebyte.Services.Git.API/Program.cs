using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework;
using Spirebyte.Services.Git.Application;
using Spirebyte.Services.Git.Core.Constants;
using Spirebyte.Services.Git.Infrastructure;
using Spirebyte.Services.Git.Infrastructure.Authentication;
using Spirebyte.Shared.IdentityServer;

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
                    options.AddEitherOrScopePolicy(ApiScopes.Read, "repositories.read", "repositories.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Write, "repositories.write", "repositories.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Delete, "repositories.delete", "repositories.manage");
                    options.AddEitherOrScopePolicy(ApiScopes.Commit, "repositories.commit", "repositories.manage");
                    options.AddPolicy(nameof(GitAuthorizeAttribute), policy =>
                    {
                        policy.AuthenticationSchemes.Add("basic-introspection");
                        policy.RequireAuthenticatedUser();
                    });
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