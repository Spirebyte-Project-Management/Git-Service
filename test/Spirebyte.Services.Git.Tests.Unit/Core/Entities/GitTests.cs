using System;
using System.Collections.Generic;
using FluentAssertions;
using Spirebyte.Services.Git.Core.Entities;
using Spirebyte.Services.Git.Core.Exceptions;
using Xunit;

namespace Spirebyte.Services.Git.Tests.Unit.Core.Entities;

public class GitTests
{
    [Fact]
    public void given_valid_input_Git_should_be_created()
    {
        var gitId = "GitKey";
        var title = "Title";
        var description = "description";
        var projectId = "projectKey";
        var createdAt = DateTime.Now;

        var git = new Git(gitId, title, description, projectId, new List<Branch>(), createdAt);

        git.Should().NotBeNull();
        git.Id.Should().Be(gitId);
        git.Title.Should().Be(title);
        git.Description.Should().Be(description);
        git.ProjectId.Should().Be(projectId);
        git.CreatedAt.Should().Be(createdAt);
    }


    [Fact]
    public void given_empty_id_Git_should_throw_an_exception()
    {
        var gitId = string.Empty;
        var title = "Title";
        var description = "description";
        var projectId = "projectKey";
        var createdAt = DateTime.Now;

        Action act = () => new Git(gitId, title, description, projectId, new List<Branch>(), createdAt);
        act.Should().Throw<InvalidIdException>();
    }

    [Fact]
    public void given_empty_projectId_Git_should_throw_an_exception()
    {
        var gitId = "GitKey";
        var title = "Title";
        var description = "description";
        var projectId = string.Empty;
        var createdAt = DateTime.Now;

        Action act = () => new Git(gitId, title, description, projectId, new List<Branch>(), createdAt);
        act.Should().Throw<InvalidProjectIdException>();
    }

    [Fact]
    public void given_empty_title_project_should_throw_an_exception()
    {
        var gitId = "GitKey";
        var title = string.Empty;
        var description = "description";
        var projectId = "projectKey";
        var createdAt = DateTime.Now;

        Action act = () => new Git(gitId, title, description, projectId, new List<Branch>(), createdAt);
        act.Should().Throw<InvalidTitleException>();
    }
}