using System;
using System.Runtime.Serialization;

namespace Spirebyte.Services.Git.Application.Exceptions.Base;

[Serializable]
public abstract class AppException : Exception
{
    protected AppException(string message) : base(message)
    {
    }

    protected AppException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
    
    public virtual string Code { get; }
}