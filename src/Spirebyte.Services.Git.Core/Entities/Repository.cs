using System;
using System.Collections.Generic;
using System.Linq;
using Spirebyte.Services.Git.Core.Exceptions;
using Spirebyte.Services.Git.Core.Helpers;

namespace Spirebyte.Services.Git.Core.Entities;

public class Repository
{
    public Repository(string id, string title, string description, string projectId, Guid referenceId,
        List<Branch> branches, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new InvalidIdException(id);

        if (string.IsNullOrWhiteSpace(projectId)) throw new InvalidProjectIdException(projectId);

        if (string.IsNullOrWhiteSpace(title)) throw new InvalidTitleException(title);

        Id = id;
        Title = title;
        Description = description;
        ProjectId = projectId;
        ReferenceId = referenceId;
        Branches = branches;
        CreatedAt = createdAt;
    }

    public void UpdateRepositoryFromGit()
    {
        var repoPath = RepoPathHelpers.GetCachePathForRepositoryId(Id, ReferenceId);
        var repoInstance = new LibGit2Sharp.Repository(repoPath);
        Branches = repoInstance.Branches.Select(b => new Branch(b)).ToList();
    }

    public Guid ChangeReferenceId()
    {
        ReferenceId = Guid.NewGuid();

        return ReferenceId;
    }
 
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    
    public Guid ReferenceId { get; set; }
    
    public List<Branch> Branches { get; set; }
    public DateTime CreatedAt { get; set; }
}