using System;
using System.Runtime.Serialization;
using Spirebyte.Services.Git.Core.Exceptions.Base;

namespace Spirebyte.Services.Git.Core.Exceptions;

[Serializable]
public class InvalidProjectIdException : DomainException
{
    public InvalidProjectIdException(string projectId) : base($"Invalid project id: {projectId}.")
    {
        ProjectId = projectId;
    }

    protected InvalidProjectIdException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public string ProjectId { get; }
    public override string Code { get; } = "invalid_project_id";
}