using System;
using FluentAssertions;
using Spirebyte.Services.Git.Core.Entities;
using Spirebyte.Services.Git.Core.Exceptions;
using Spirebyte.Services.Git.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Git.Tests.Unit.Core.Entities;

public class RepositoryTests
{
    [Fact]
    public void given_valid_input_repository_should_be_created()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        var repository = new Repository(fakedRepository.Id, fakedRepository.Title, fakedRepository.Description,
            fakedRepository.ProjectId, fakedRepository.ReferenceId, fakedRepository.Branches,
            fakedRepository.CreatedAt);

        repository.Should().NotBeNull();
        repository.Id.Should().Be(fakedRepository.Id);
        repository.Title.Should().Be(fakedRepository.Title);
        repository.Description.Should().Be(fakedRepository.Description);
        repository.ProjectId.Should().Be(fakedRepository.ProjectId);
        repository.ReferenceId.Should().Be(fakedRepository.ReferenceId);
        repository.Branches.Should().Equal(fakedRepository.Branches);
        repository.CreatedAt.Should().Be(fakedRepository.CreatedAt);
    }


    [Fact]
    public void given_empty_id_repository_should_throw_an_exception()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        Action act = () => new Repository(string.Empty, fakedRepository.Title, fakedRepository.Description,
            fakedRepository.ProjectId, fakedRepository.ReferenceId, fakedRepository.Branches,
            fakedRepository.CreatedAt);
        act.Should().Throw<InvalidIdException>();
    }

    [Fact]
    public void given_empty_projectId_repository_should_throw_an_exception()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        Action act = () => new Repository(fakedRepository.Id, fakedRepository.Title, fakedRepository.Description,
            string.Empty, fakedRepository.ReferenceId, fakedRepository.Branches, fakedRepository.CreatedAt);
        act.Should().Throw<InvalidProjectIdException>();
    }

    [Fact]
    public void given_empty_title_repository_should_throw_an_exception()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        Action act = () => new Repository(fakedRepository.Id, string.Empty, fakedRepository.Description,
            fakedRepository.ProjectId, fakedRepository.ReferenceId, fakedRepository.Branches,
            fakedRepository.CreatedAt);
        act.Should().Throw<InvalidTitleException>();
    }
}