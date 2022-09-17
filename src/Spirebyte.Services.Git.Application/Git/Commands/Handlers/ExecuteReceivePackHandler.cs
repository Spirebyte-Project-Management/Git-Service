using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using LibGit2Sharp;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Git.Application.Clients.Interfaces;
using Spirebyte.Services.Git.Application.Exceptions;
using Spirebyte.Services.Git.Application.Git.Services.Interfaces;
using Spirebyte.Services.Git.Application.Projects.Exceptions;
using Spirebyte.Services.Git.Core.Constants;
using Spirebyte.Services.Git.Core.Helpers;
using Spirebyte.Services.Git.Core.Repositories;

namespace Spirebyte.Services.Git.Application.Git.Commands.Handlers;

public class ExecuteReceivePackHandler : ICommandHandler<ExecuteReceivePack>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryService _repositoryService;

    public ExecuteReceivePackHandler(IProjectRepository projectRepository, IRepositoryRepository repositoryRepository,
        IRepositoryService repositoryService, IProjectsApiHttpClient projectsApiHttpClient, IContextAccessor contextAccessor)
    {
        _projectRepository = projectRepository;
        _repositoryRepository = repositoryRepository;
        _repositoryService = repositoryService;
        _projectsApiHttpClient = projectsApiHttpClient;
        _contextAccessor = contextAccessor;
    }

    public async Task HandleAsync(ExecuteReceivePack command, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(command.ProjectId);
        if (project is null) throw new ProjectNotFoundException(command.ProjectId);

        if (!await _projectsApiHttpClient.HasPermission(RepositoryPermissionKeys.Commit, _contextAccessor.Context.GetUserId(),
                command.ProjectId)) throw new ActionNotAllowedException();

        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        await _repositoryService.EnsureLatestRepositoryIsCached(repository);

        await Cli.Wrap("git")
            .WithArguments(builder => builder
                .Add("receive-pack")
                .Add("--stateless-rpc")
                .Add(RepoPathHelpers.GetCachePathForRepository(repository)))
            .WithWorkingDirectory(RepoPathHelpers.RepoCacheDirPath)
            .WithStandardInputPipe(PipeSource.FromStream(command.InputStream))
            .WithStandardOutputPipe(PipeTarget.ToStream(command.OutputStream, true))
            .ExecuteAsync(cancellationToken);

        await repository.UpdateRepositoryFromGit();

        repository = await _repositoryService.UploadRepoChanges(repository);

        await _repositoryRepository.UpdateAsync(repository);
    }
}