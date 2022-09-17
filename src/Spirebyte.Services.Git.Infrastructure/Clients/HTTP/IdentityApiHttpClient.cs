using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Spirebyte.Framework.HTTP;
using Spirebyte.Services.Git.Application.Clients.DTO;
using Spirebyte.Services.Git.Application.Clients.Interfaces;

namespace Spirebyte.Services.Git.Infrastructure.Clients.HTTP;

internal sealed class IdentityApiHttpClient : IIdentityApiHttpClient
{
    private readonly string _clientName;
    private readonly IHttpClientFactory _factory;
    private readonly string _url;
    public IdentityApiHttpClient(IHttpClientFactory factory, IOptions<HttpClientOptions> options)
    {
        _factory = factory;
        _clientName = options.Value.Name;
        _url = options.Value.Services["identity"];
    }

    public Task<UserDto> GetUserAsync(Guid userId)
    {
        return _factory.CreateClient(_clientName)
            .GetFromJsonAsync<UserDto>($"{_url}/users/{userId}/");
    }
}