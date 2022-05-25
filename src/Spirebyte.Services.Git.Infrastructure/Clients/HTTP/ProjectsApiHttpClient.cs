using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Spirebyte.Services.Git.Application.Clients.Interfaces;

namespace Spirebyte.Services.Git.Infrastructure.Clients.HTTP;

internal sealed class ProjectsApiHttpClient : IProjectsApiHttpClient
{
    private readonly IHttpClient _client;
    private readonly string _url;

    public ProjectsApiHttpClient(IHttpClient client, HttpClientOptions options)
    {
        _client = client;
        _url = options.Services["projects"];
    }

    public Task<bool> HasPermission(string permissionKey, Guid userId, string projectId)
    {
        return _client.GetAsync<bool>($"{_url}/projects/{projectId}/user/{userId}/hasPermission/{permissionKey}/");
    }
}