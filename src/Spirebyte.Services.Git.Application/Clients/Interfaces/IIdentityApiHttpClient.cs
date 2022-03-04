using System;
using System.Threading.Tasks;
using Spirebyte.Services.Git.Application.Clients.DTO;

namespace Spirebyte.Services.Git.Application.Clients.Interfaces;

public interface IIdentityApiHttpClient
{
    Task<UserDto> GetUserAsync(Guid userId);
}