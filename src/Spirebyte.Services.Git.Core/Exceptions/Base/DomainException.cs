using System;
using System.Runtime.Serialization;

namespace Spirebyte.Services.Git.Core.Exceptions.Base;

[Serializable]
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
    
    protected DomainException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
    public virtual string Code { get; }
}