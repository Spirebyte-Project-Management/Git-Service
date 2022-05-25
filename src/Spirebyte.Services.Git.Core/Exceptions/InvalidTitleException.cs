using System;
using System.Runtime.Serialization;
using Spirebyte.Services.Git.Core.Exceptions.Base;

namespace Spirebyte.Services.Git.Core.Exceptions;

[Serializable]
public class InvalidTitleException : DomainException
{
    public InvalidTitleException(string title) : base($"Invalid title: {title}.")
    {
    }

    protected InvalidTitleException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override string Code { get; } = "invalid_title";
}