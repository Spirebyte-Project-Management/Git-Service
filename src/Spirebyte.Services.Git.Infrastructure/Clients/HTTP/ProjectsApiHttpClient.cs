using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Spirebyte.Framework.HTTP;
using Spirebyte.Services.Git.Application.Clients.Interfaces;

namespace Spirebyte.Services.Git.Infrastructure.Clients.HTTP;

internal sealed class ProjectsApiHttpClient : IProjectsApiHttpClient
{
    private readonly string _clientName;
    private readonly IHttpClientFactory _factory;
    private readonly string _url;
    public ProjectsApiHttpClient(IHttpClientFactory factory, IOptions<HttpClientOptions> options)
    {
        _factory = factory;
        _clientName = options.Value.Name;
        _url = options.Value.Services["projects"];
    }

    public Task<bool> HasPermission(string permissionKey, Guid userId, string projectId)
    {
        return _factory.CreateClient(_clientName)
            .GetFromJsonAsync<bool>($"{_url}/projects/{projectId}/user/{userId}/hasPermission/{permissionKey}/");
    }
}