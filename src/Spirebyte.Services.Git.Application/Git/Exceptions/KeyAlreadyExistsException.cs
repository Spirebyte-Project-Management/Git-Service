using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Git.Application.Git.Exceptions;

[Serializable]
public class KeyAlreadyExistsException : AppException
{
    public KeyAlreadyExistsException(string gitId)
        : base($"Git with id: {gitId} already exists.")
    {
        GitId = gitId;
    }
    public string GitId { get; }
}