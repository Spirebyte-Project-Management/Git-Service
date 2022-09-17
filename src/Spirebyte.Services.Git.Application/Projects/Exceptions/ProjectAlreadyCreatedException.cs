using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Git.Application.Projects.Exceptions;

public class ProjectAlreadyCreatedException : AppException
{
    public ProjectAlreadyCreatedException(string projectId)
        : base($"Project with id: {projectId} was already created.")
    {
        ProjectId = projectId;
    }
    public string ProjectId { get; }
}