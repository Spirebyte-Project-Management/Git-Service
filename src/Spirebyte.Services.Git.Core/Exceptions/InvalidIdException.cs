using System;
using System.Runtime.Serialization;
using Spirebyte.Services.Git.Core.Exceptions.Base;

namespace Spirebyte.Services.Git.Core.Exceptions;

[Serializable]
public class InvalidIdException : DomainException
{
    public InvalidIdException(string key) : base($"Invalid key: {key}.")
    {
    }
    protected InvalidIdException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
    public override string Code { get; } = "invalid_key";
}