using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using Convey.CQRS.Commands;
using LibGit2Sharp;
using Spirebyte.Services.Git.Application.Git.Services;
using Spirebyte.Services.Git.Application.Git.Services.Interfaces;
using Spirebyte.Services.Git.Application.Projects.Exceptions;
using Spirebyte.Services.Git.Core.Helpers;
using Spirebyte.Services.Git.Core.Repositories;

namespace Spirebyte.Services.Git.Application.Git.Commands.Handlers;

public class ExecuteUploadPackHandler : ICommandHandler<ExecuteUploadPack>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryService _repositoryService;

    public ExecuteUploadPackHandler(IProjectRepository projectRepository, IRepositoryRepository repositoryRepository, IRepositoryService repositoryService)
    {
        _projectRepository = projectRepository;
        _repositoryRepository = repositoryRepository;
        _repositoryService = repositoryService;
    }
    
    public async Task HandleAsync(ExecuteUploadPack command, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(command.ProjectId);
        if (project is null) throw new ProjectNotFoundException(command.ProjectId);
        
        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        await _repositoryService.EnsureLatestRepositoryIsCached(repository);
        
        await Cli.Wrap("git")
            .WithArguments(builder => builder
                .Add("upload-pack")
                .Add("--stateless-rpc")
                .Add(RepoPathHelpers.GetCachePathForRepository(repository)))
            .WithWorkingDirectory(RepoPathHelpers.RepoCacheDirPath)
            .WithStandardInputPipe(PipeSource.FromStream(command.InputStream))
            .WithStandardOutputPipe(PipeTarget.ToStream(command.OutputStream, true))
            .ExecuteAsync(cancellationToken);
    }
}