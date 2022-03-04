using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Git.API;
using Spirebyte.Services.Git.Application.Git.DTO;
using Spirebyte.Services.Git.Application.Git.Queries;
using Spirebyte.Services.Git.Core.Entities;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Git.Tests.Shared.Factories;
using Spirebyte.Services.Git.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Git.Tests.Integration.Queries;

[Collection("Spirebyte collection")]
public class GetGitTests : IDisposable
{
    private const string Exchange = "Git";
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly IQueryHandler<GetGit, IEnumerable<GitDto>> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<GitDocument, string> _gitMongoDbFixture;

    public GetGitTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _gitMongoDbFixture = new MongoDbFixture<GitDocument, string>("Git");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        factory.Server.AllowSynchronousIO = true;
        _queryHandler =
            factory.Services.GetRequiredService<IQueryHandler<GetGit, IEnumerable<GitDto>>>();
    }

    public async void Dispose()
    {
        _gitMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task get_Git_query_succeeds_when_a_Git_exists()
    {
        var gitId = "GitKey" + Guid.NewGuid();
        var git2Id = "Git2Key" + Guid.NewGuid();
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        var createdAt = DateTime.Now;
        
        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());

        var git = new Git(gitId, title, description, projectId, new List<Branch>(), createdAt);
        var git2 = new Git(git2Id, title, description, projectId, new List<Branch>(), createdAt);

        await _gitMongoDbFixture.InsertAsync(git.AsDocument());
        await _gitMongoDbFixture.InsertAsync(git2.AsDocument());


        var query = new GetGit(projectId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        var gitDtos = result as GitDto[] ?? result.ToArray();
        gitDtos.Should().Contain(i => i.Id == gitId);
        gitDtos.Should().Contain(i => i.Id == git2Id);
    }

    [Fact]
    public async Task get_Git_query_should_return_empty_when_no_Git_exist()
    {
        var query = new GetGit();

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeEmpty();
    }
}