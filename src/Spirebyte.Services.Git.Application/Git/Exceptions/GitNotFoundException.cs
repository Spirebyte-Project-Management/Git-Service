using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Git.Application.Git.Exceptions;

[Serializable]
public class GitNotFoundException : AppException
{
    public GitNotFoundException(string key) : base($"Git with Key: '{key}' was not found.")
    {
        Key = key;
    }
    public string Key { get; }
}