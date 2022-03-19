using System;
using System.IO;
using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Core.Helpers;

public static class RepoPathHelpers
{
    private const string ReferenceFileName = "REFERENCE";
    public static readonly string RepoCacheDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RepoCache");

    public static string GetCachePathForRepository(Repository repository)
    {
        return GetCachePathForRepositoryId(repository.Id);
    }
    
    public static string GetCachePathForRepositoryId(string repositoryId)
    {
        return Path.Combine(RepoCacheDirPath, repositoryId);
    }

    public static bool RepoCacheIsCurrentReference(Repository repository)
    {
        var cachePath = GetCachePathForRepository(repository);
        if (!Directory.Exists(cachePath)) return false;
        
        var currentReference = File.ReadAllText(Path.Combine(cachePath, ReferenceFileName));

        return currentReference == repository.ReferenceId.ToString();
    }
    
    public static void UpdateRepoCacheReference(string repositoryId, Guid referenceId)
    {
        var cachePath = GetCachePathForRepositoryId(repositoryId);
        File.WriteAllText(Path.Combine(cachePath, ReferenceFileName), referenceId.ToString());
    }
    
    public static string GetUploadPathForRepo(Repository repository)
    {
        return $"{repository.ProjectId}/{repository.Id}";
    }
}