using System;
using System.Runtime.Serialization;
using Spirebyte.Services.Git.Application.Exceptions.Base;

namespace Spirebyte.Services.Git.Application.Git.Exceptions;

[Serializable]
public class KeyAlreadyExistsException : AppException
{
    public KeyAlreadyExistsException(string gitId)
        : base($"Git with id: {gitId} already exists.")
    {
        GitId = gitId;
    }

    protected KeyAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Code { get; } = "key_already_exists";
    public string GitId { get; }
}