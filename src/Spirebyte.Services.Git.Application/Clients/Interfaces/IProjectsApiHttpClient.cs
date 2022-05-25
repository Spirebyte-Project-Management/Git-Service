using System;
using System.Threading.Tasks;
using Convey.HTTP;

namespace Spirebyte.Services.Git.Application.Clients.Interfaces;

public interface IProjectsApiHttpClient
{
    Task<bool> HasPermission(string permissionKey, Guid userId, string projectId);
}