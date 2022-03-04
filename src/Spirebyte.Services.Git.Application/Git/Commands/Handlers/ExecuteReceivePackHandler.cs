using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using Convey.CQRS.Commands;
using LibGit2Sharp;
using Spirebyte.Services.Git.Application.Git.Helpers;
using Spirebyte.Services.Git.Application.Projects.Exceptions;
using Spirebyte.Services.Git.Core.Repositories;

namespace Spirebyte.Services.Git.Application.Git.Commands.Handlers;

public class ExecuteReceivePackHandler : ICommandHandler<ExecuteReceivePack>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public ExecuteReceivePackHandler(IProjectRepository projectRepository, IRepositoryRepository repositoryRepository)
    {
        _projectRepository = projectRepository;
        _repositoryRepository = repositoryRepository;
    }
    
    public async Task HandleAsync(ExecuteReceivePack command, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(command.ProjectId);
        if (project is null) throw new ProjectNotFoundException(command.ProjectId);
        
        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        await Cli.Wrap("git")
            .WithArguments(builder => builder
                .Add("receive-pack")
                .Add("--stateless-rpc")
                .Add(RepoPathHelpers.GetCachePathForRepositoryId(command.RepositoryId)))
            .WithWorkingDirectory(RepoPathHelpers.RepoCacheDirPath)
            .WithStandardInputPipe(PipeSource.FromStream(command.InputStream))
            .WithStandardOutputPipe(PipeTarget.ToStream(command.OutputStream, true))
            .ExecuteAsync(cancellationToken);
    }
}