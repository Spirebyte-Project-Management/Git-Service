using System.IO;
using System.Threading.Tasks;
using Partytitan.Convey.Minio.Services.Interfaces;
using Spirebyte.Services.Git.Application.Git.Services.Interfaces;
using Spirebyte.Services.Git.Core.Entities;
using Spirebyte.Services.Git.Core.Helpers;

namespace Spirebyte.Services.Git.Application.Git.Services;

public class RepositoryService : IRepositoryService
{
    private readonly IMinioService _minioService;

    public RepositoryService(IMinioService minioService)
    {
        _minioService = minioService;
    }

    public async Task EnsureLatestRepositoryIsCached(Repository repository)
    {
        var repoPath = RepoPathHelpers.GetCachePathForRepository(repository);
        
        if (Directory.Exists(repoPath)) return;

        if (repoPath != null)
        {
            var parentDir = Directory.GetParent(repoPath);
            if (parentDir != null)
            {
                if (!parentDir.Exists)
                {
                    parentDir.Create();
                }
                
                parentDir.Refresh();

                foreach (var dir in parentDir.EnumerateDirectories())
                {
                    ForceDeleteDirectory(dir.FullName);
                }
            }

            await _minioService.DownloadDirAsync(repoPath, RepoPathHelpers.GetUploadPathForRepo(repository));

            EnsureRequiredGitDirectoriesExist(repoPath);
        }
    }

    public async Task<Repository> UploadRepoChanges(Repository repository)
    {
        var repoPath = RepoPathHelpers.GetCachePathForRepository(repository);
        
        if (!Directory.Exists(repoPath)) return repository;

        await _minioService.UploadDirAsync(repoPath, RepoPathHelpers.GetUploadPathForRepo(repository));

        var newReferenceId = repository.ChangeReferenceId();
        
        Directory.Move(repoPath, RepoPathHelpers.GetCachePathForRepositoryId(repository.Id, newReferenceId));

        return repository;
    }

    private static void ForceDeleteDirectory(string path) 
    {
        var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

        foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
        {
            info.Attributes = FileAttributes.Normal;
        }

        directory.Delete(true);
    }
    
    private void EnsureRequiredGitDirectoriesExist(string repoPath)
    {
        var refsPath = Path.Combine(repoPath, "refs");
        var refsHeadPath = Path.Combine(refsPath, "heads");
        var refsTagsPath = Path.Combine(refsPath, "tags");
        if (!Directory.Exists(refsPath))
        {
            Directory.CreateDirectory(refsPath);
        }
        if (!Directory.Exists(refsHeadPath))
        {
            Directory.CreateDirectory(refsHeadPath);
        }
        if (!Directory.Exists(refsTagsPath))
        {
            Directory.CreateDirectory(refsTagsPath);
        }
        var objectsPath = Path.Combine(repoPath, "objects");
        var objectsInfoPath = Path.Combine(objectsPath, "info");
        var objectsPackPath = Path.Combine(objectsPath, "pack");
        if (!Directory.Exists(objectsPath))
        {
            Directory.CreateDirectory(objectsPath);
        }
        if (!Directory.Exists(objectsInfoPath))
        {
            Directory.CreateDirectory(objectsInfoPath);
        }
        if (!Directory.Exists(objectsPackPath))
        {
            Directory.CreateDirectory(objectsPackPath);
        }
    }
}