using System;
using System.Runtime.Serialization;
using Spirebyte.Services.Git.Application.Exceptions.Base;

namespace Spirebyte.Services.Git.Application.Git.Exceptions;

[Serializable]
public class GitNotFoundException : AppException
{
    public GitNotFoundException(string key) : base($"Git with Key: '{key}' was not found.")
    {
        Key = key;
    }

    protected GitNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Code { get; } = "Git_not_found";
    public string Key { get; }
}