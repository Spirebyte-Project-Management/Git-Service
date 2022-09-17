using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.DAL.MongoDb;
using Spirebyte.Services.Git.Application.Clients.Interfaces;
using Spirebyte.Services.Git.Core.Repositories;
using Spirebyte.Services.Git.Infrastructure.Clients.HTTP;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Git.Infrastructure.Mongo.Repositories;

namespace Spirebyte.Services.Git.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IIdentityApiHttpClient, IdentityApiHttpClient>();
        services.AddTransient<IProjectsApiHttpClient, ProjectsApiHttpClient>();

        services.AddMongo(configuration)
            .AddMongoRepository<ProjectDocument, string>("projects")
            .AddMongoRepository<RepositoryDocument, string>("repositories");

        services.AddTransient<IRepositoryRepository, RepositoryRepository>();
        services.AddTransient<IProjectRepository, ProjectRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        return builder;
    }
}