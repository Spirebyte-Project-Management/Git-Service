using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Git.API;
using Spirebyte.Services.Git.Application.Projects.Exceptions;
using Spirebyte.Services.Git.Application.Git.Commands;
using Spirebyte.Services.Git.Core.Entities;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Git.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Git.Tests.Shared.Factories;
using Spirebyte.Services.Git.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Git.Tests.Integration.Commands;

[Collection("Spirebyte collection")]
public class CreateGitTests : IDisposable
{
    private const string Exchange = "Git";
    private readonly ICommandHandler<CreateGit> _commandHandler;
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<GitDocument, string> _gitMongoDbFixture;

    public CreateGitTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _gitMongoDbFixture = new MongoDbFixture<GitDocument, string>("Git");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<CreateGit>>();
    }

    public async void Dispose()
    {
        _gitMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task create_Git_command_should_add_Git_with_given_data_to_database()
    {
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        
        var expectedGitKey = $"{projectId}-Git-1";

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new CreateGit(title, description, projectId);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var git = await _gitMongoDbFixture.GetAsync(expectedGitKey);

        git.Should().NotBeNull();
        git.Id.Should().Be(expectedGitKey);
        git.Title.Should().Be(title);
        git.Description.Should().Be(description);
        git.ProjectId.Should().Be(projectId);
    }

    [Fact]
    public async void create_Git_command_fails_when_project_does_not_exist()
    {
        var title = "Title";
        var description = "description";
        var projectId = "projectId" + Guid.NewGuid();

        var command = new CreateGit(title, description, projectId);


        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }
}