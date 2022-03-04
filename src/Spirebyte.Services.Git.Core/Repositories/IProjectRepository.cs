using System.Threading.Tasks;
using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Core.Repositories;

public interface IProjectRepository
{
    Task<Project> GetAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task AddAsync(Project project);
}