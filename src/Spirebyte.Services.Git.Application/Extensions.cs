using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Git.Application.Git.Services;
using Spirebyte.Services.Git.Application.Git.Services.Interfaces;

namespace Spirebyte.Services.Git.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IRepositoryService, RepositoryService>();
        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        return app;
    }
}