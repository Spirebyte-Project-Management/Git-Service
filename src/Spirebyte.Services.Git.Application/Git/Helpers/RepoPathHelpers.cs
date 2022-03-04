using System;
using System.IO;
using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Application.Git.Helpers;

public static class RepoPathHelpers
{
    public static readonly string RepoCacheDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RepoCache");

    public static string GetCachePathForRepository(Repository repository)
    {
        return GetCachePathForRepositoryId(repository.Id);
    }
    
    public static string GetCachePathForRepositoryId(string repositoryId)
    {
        return Path.Combine(RepoCacheDirPath, repositoryId);
    }
}