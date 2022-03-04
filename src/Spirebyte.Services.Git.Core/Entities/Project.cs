using System.Collections.Generic;

namespace Spirebyte.Services.Git.Core.Entities;

public class Project
{
    public Project(string id)
    {
        Id = id;
    }

    public string Id { get; }
    public ICollection<string> Issues { get; set; }
}