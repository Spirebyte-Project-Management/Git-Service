using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Spirebyte.Services.Git.Core.Entities;
using Spirebyte.Services.Git.Core.Repositories;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents.Mappers;

namespace Spirebyte.Services.Git.Infrastructure.Mongo.Repositories;

internal sealed class ProjectRepository : IProjectRepository
{
    private readonly IMongoRepository<ProjectDocument, string> _repository;

    public ProjectRepository(IMongoRepository<ProjectDocument, string> repository)
    {
        _repository = repository;
    }

    public async Task<Project> GetAsync(string id)
    {
        var project = await _repository.GetAsync(id);

        return project?.AsEntity();
    }

    public Task<bool> ExistsAsync(string id)
    {
        return _repository.ExistsAsync(c => c.Id == id);
    }

    public Task AddAsync(Project project)
    {
        return _repository.AddAsync(project.AsDocument());
    }
}