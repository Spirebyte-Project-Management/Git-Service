using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using Convey.CQRS.Commands;
using LibGit2Sharp;
using Spirebyte.Services.Git.Application.Clients.Interfaces;
using Spirebyte.Services.Git.Application.Exceptions;
using Spirebyte.Services.Git.Application.Git.Services;
using Spirebyte.Services.Git.Application.Git.Services.Interfaces;
using Spirebyte.Services.Git.Application.Projects.Exceptions;
using Spirebyte.Services.Git.Core.Constants;
using Spirebyte.Services.Git.Core.Helpers;
using Spirebyte.Services.Git.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Git.Application.Git.Commands.Handlers;

public class ExecuteUploadPackHandler : ICommandHandler<ExecuteUploadPack>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryService _repositoryService;
    private readonly IAppContext _appContext;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;

    public ExecuteUploadPackHandler(IProjectRepository projectRepository, IRepositoryRepository repositoryRepository,
        IRepositoryService repositoryService, IAppContext appContext, IProjectsApiHttpClient projectsApiHttpClient)
    {
        _projectRepository = projectRepository;
        _repositoryRepository = repositoryRepository;
        _repositoryService = repositoryService;
        _appContext = appContext;
        _projectsApiHttpClient = projectsApiHttpClient;
    }
    
    public async Task HandleAsync(ExecuteUploadPack command, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(command.ProjectId);
        if (project is null) throw new ProjectNotFoundException(command.ProjectId);
        
        if (!await _projectsApiHttpClient.HasPermission(RepositoryPermissionKeys.Commit, _appContext.Identity.Id,
                command.ProjectId)) throw new ActionNotAllowedException();
        
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