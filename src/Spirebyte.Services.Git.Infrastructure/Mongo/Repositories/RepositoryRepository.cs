using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Spirebyte.Services.Git.Core.Entities;
using Spirebyte.Services.Git.Core.Repositories;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Git.Infrastructure.Mongo.Repositories;

internal sealed class RepositoryRepository : IRepositoryRepository
{
    private readonly IMongoRepository<RepositoryDocument, string> _repository;

    public RepositoryRepository(IMongoRepository<RepositoryDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<Repository> GetAsync(string repositoryId)
    {
        var repository = await _repository.GetAsync(x => x.Id == repositoryId);

        return repository?.AsEntity();
    }

    public async Task<Repository> GetLatest()
    {
        var documents = _repository.Collection.AsQueryable();

        var repositories = await documents.ToListAsync();

        var repository = repositories.OrderByDescending(c => c.CreatedAt).FirstOrDefault();
        return repository.AsEntity();
    }

    public Task<long> GetRepositoryCountOfProjectAsync(string projectId)
    {
        return _repository.Collection.CountDocumentsAsync(x => x.ProjectId == projectId);
    }

    public Task<bool> ExistsAsync(string repositoryId)
    {
        return _repository.ExistsAsync(c => c.Id == repositoryId);
    }

    public Task AddAsync(Repository repository)
    {
        return _repository.AddAsync(repository.AsDocument());
    }

    public async Task UpdateAsync(Repository repository)
    {
        await _repository.UpdateAsync(repository.AsDocument());
    }

    public Task DeleteAsync(string repositoryId)
    {
        return _repository.DeleteAsync(repositoryId);
    }
}