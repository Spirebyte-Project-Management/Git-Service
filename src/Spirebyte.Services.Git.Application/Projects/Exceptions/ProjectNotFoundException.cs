using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Git.Application.Projects.Exceptions;

public class ProjectNotFoundException : AppException
{
    public ProjectNotFoundException(string projectId) : base($"Project with Id: '{projectId}' was not found.")
    {
        ProjectId = projectId;
    }
    public string ProjectId { get; }
}