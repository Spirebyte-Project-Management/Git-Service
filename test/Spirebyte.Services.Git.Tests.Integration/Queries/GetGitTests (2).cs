using System;
using System.Collections.Generic;
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
    private readonly IQueryHandler<GetGit, GitDto> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<GitDocument, string> _gitMongoDbFixture;

    public GetGitTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _gitMongoDbFixture = new MongoDbFixture<GitDocument, string>("Git");
        factory.Server.AllowSynchronousIO = true;
        _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetGit, GitDto>>();
    }

    public async void Dispose()
    {
        _gitMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task get_Git_query_succeeds_when_Git_exists()
    {
        var gitId = "GitKey" + Guid.NewGuid();
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        var createdAt = DateTime.Now;

        var git = new Git(gitId, title, description, projectId, new List<Branch>(), createdAt);

        await _gitMongoDbFixture.InsertAsync(git.AsDocument());


        var query = new GetGit(gitId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().NotBeNull();
        result.Id.Should().Be(gitId);
        result.Title.Should().Be(title);
        result.Description.Should().Be(description);
    }

    [Fact]
    public async Task get_Git_query_should_return_null_when_Git_does_not_exist()
    {
        var gitId = "notExistingGitKey";

        var query = new GetGit(gitId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeNull();
    }
}