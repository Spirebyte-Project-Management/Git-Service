using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Git.Core.Exceptions;

[Serializable]
public class InvalidProjectIdException : DomainException
{
    public InvalidProjectIdException(string projectId) : base($"Invalid project id: {projectId}.")
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; }
}