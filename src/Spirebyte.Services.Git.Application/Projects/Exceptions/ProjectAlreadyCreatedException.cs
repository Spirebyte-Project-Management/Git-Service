using Spirebyte.Services.Git.Application.Exceptions.Base;

namespace Spirebyte.Services.Git.Application.Projects.Exceptions;

public class ProjectAlreadyCreatedException : AppException
{
    public ProjectAlreadyCreatedException(string projectId)
        : base($"Project with id: {projectId} was already created.")
    {
        ProjectId = projectId;
    }

    public override string Code { get; } = "project_already_created";
    public string ProjectId { get; }
}