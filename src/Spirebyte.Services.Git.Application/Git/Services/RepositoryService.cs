using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.FileStorage.S3.Services;
using Spirebyte.Services.Git.Application.Git.Services.Interfaces;
using Spirebyte.Services.Git.Core.Entities;
using Spirebyte.Services.Git.Core.Helpers;

namespace Spirebyte.Services.Git.Application.Git.Services;

public class RepositoryService : IRepositoryService
{
    private readonly IS3Service _s3Service;

    public RepositoryService(IS3Service s3Service)
    {
        _s3Service = s3Service;
    }

    public async Task EnsureLatestRepositoryIsCached(Repository repository)
    {
        if (RepoPathHelpers.RepoCacheIsCurrentReference(repository)) return;

        var repoPath = RepoPathHelpers.GetCachePathForRepository(repository);

        if (repoPath != null)
        {
            var repoDir = new DirectoryInfo(repoPath);

            if (!repoDir.Exists)
            {
                repoDir.Create();
                repoDir.Refresh();
            }

            foreach (var dir in repoDir.EnumerateDirectories()) ForceDeleteDirectory(dir.FullName);

            await _s3Service.DownloadDirAsync(repoPath, RepoPathHelpers.GetUploadPathForRepo(repository));

            EnsureRequiredGitDirectoriesExist(repoPath);
        }
    }

    public async Task<Repository> UploadRepoChanges(Repository repository)
    {
        var repoPath = RepoPathHelpers.GetCachePathForRepository(repository);

        if (!Directory.Exists(repoPath)) return repository;

        while (File.Exists(Path.Combine(repoPath, "index.lock")))
            // do something, for example wait a second
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

        var newReferenceId = repository.ChangeReferenceId();
        RepoPathHelpers.UpdateRepoCacheReference(repository.Id, newReferenceId);

        await _s3Service.UploadDirAsync(repoPath, RepoPathHelpers.GetUploadPathForRepo(repository));

        return repository;
    }

    private static void ForceDeleteDirectory(string path)
    {
        var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

        foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            info.Attributes = FileAttributes.Normal;

        directory.Delete(true);
    }

    private void EnsureRequiredGitDirectoriesExist(string repoPath)
    {
        var refsPath = Path.Combine(repoPath, "refs");
        var refsHeadPath = Path.Combine(refsPath, "heads");
        var refsTagsPath = Path.Combine(refsPath, "tags");
        if (!Directory.Exists(refsPath)) Directory.CreateDirectory(refsPath);
        if (!Directory.Exists(refsHeadPath)) Directory.CreateDirectory(refsHeadPath);
        if (!Directory.Exists(refsTagsPath)) Directory.CreateDirectory(refsTagsPath);
        var objectsPath = Path.Combine(repoPath, "objects");
        var objectsInfoPath = Path.Combine(objectsPath, "info");
        var objectsPackPath = Path.Combine(objectsPath, "pack");
        if (!Directory.Exists(objectsPath)) Directory.CreateDirectory(objectsPath);
        if (!Directory.Exists(objectsInfoPath)) Directory.CreateDirectory(objectsInfoPath);
        if (!Directory.Exists(objectsPackPath)) Directory.CreateDirectory(objectsPackPath);
    }
}