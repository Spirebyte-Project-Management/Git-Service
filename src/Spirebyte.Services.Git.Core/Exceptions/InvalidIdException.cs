using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Git.Core.Exceptions;

[Serializable]
public class InvalidIdException : DomainException
{
    public InvalidIdException(string key) : base($"Invalid key: {key}.")
    {
    }
}