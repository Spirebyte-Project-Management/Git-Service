using System.Threading.Tasks;
using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Core.Repositories;

public interface IRepositoryRepository
{
    Task<Repository> GetAsync(string repositoryId);
    Task<long> GetRepositoryCountOfProjectAsync(string projectId);
    Task<Repository> GetLatest();
    Task<bool> ExistsAsync(string repositoryId);
    Task AddAsync(Repository repository);
    Task UpdateAsync(Repository repository);

    Task DeleteAsync(string repositoryId);
}