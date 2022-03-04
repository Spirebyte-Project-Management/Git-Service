using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using Convey.CQRS.Commands;
using LibGit2Sharp;
using Spirebyte.Services.Git.Application.Git.Helpers;
using Spirebyte.Services.Git.Application.Projects.Exceptions;
using Spirebyte.Services.Git.Core.Repositories;

namespace Spirebyte.Services.Git.Application.Git.Commands.Handlers;

public class GetInfoRefsHandler : ICommandHandler<GetInfoRefs>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public GetInfoRefsHandler(IProjectRepository projectRepository, IRepositoryRepository repositoryRepository)
    {
        _projectRepository = projectRepository;
        _repositoryRepository = repositoryRepository;
    }
    
    public async Task HandleAsync(GetInfoRefs request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(request.ProjectId);
        if (project is null) throw new ProjectNotFoundException(request.ProjectId);
        
        var repository = await _repositoryRepository.GetAsync(request.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(request.RepositoryId);

        await Cli.Wrap("git")
            .WithArguments(builder => builder
                .Add(request.Service)
                .Add("--stateless-rpc")
                .Add("--advertise-refs")
                .Add(RepoPathHelpers.GetCachePathForRepositoryId(request.RepositoryId)))
            .WithWorkingDirectory(RepoPathHelpers.RepoCacheDirPath)
            .WithStandardInputPipe(PipeSource.FromStream(request.InputStream))
            .WithStandardOutputPipe(PipeTarget.ToStream(request.OutputStream, true))
            .ExecuteAsync(cancellationToken);
    }
}