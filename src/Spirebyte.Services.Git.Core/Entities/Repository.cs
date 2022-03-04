using System;
using System.Collections.Generic;
using Spirebyte.Services.Git.Core.Exceptions;

namespace Spirebyte.Services.Git.Core.Entities;

public class Repository
{
    public Repository(string id, string title, string description, string projectId, IEnumerable<Branch> branches,
        DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new InvalidIdException(id);

        if (string.IsNullOrWhiteSpace(projectId)) throw new InvalidProjectIdException(projectId);

        if (string.IsNullOrWhiteSpace(title)) throw new InvalidTitleException(title);

        Id = id;
        Title = title;
        Description = description;
        ProjectId = projectId;
        Branches = branches;
        CreatedAt = createdAt;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    
    public IEnumerable<Branch> Branches { get; set; }
    public DateTime CreatedAt { get; set; }
}