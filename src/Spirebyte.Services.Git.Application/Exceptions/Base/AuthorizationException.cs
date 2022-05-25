using System;
using System.Runtime.Serialization;

namespace Spirebyte.Services.Git.Application.Exceptions.Base;

[Serializable]
public abstract class AuthorizationException : Exception
{
    protected AuthorizationException(string message) : base(message)
    {
    }

    protected AuthorizationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public virtual string Code { get; }
}