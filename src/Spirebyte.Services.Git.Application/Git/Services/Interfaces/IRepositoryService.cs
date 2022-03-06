using System.Threading.Tasks;
using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Application.Git.Services.Interfaces;

public interface IRepositoryService
{
    Task EnsureLatestRepositoryIsCached(Repository repository);
    Task<Repository> UploadRepoChanges(Repository repository);
}