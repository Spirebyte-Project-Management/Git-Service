using System.Collections;
using System.Linq;

namespace Spirebyte.Services.Git.Core.Entities;

public class Branch
{
    public Branch(string id, string name, IEnumerable commits)
    {
        Id = id;
        Name = name;
        Commits = commits;
    }
    
    public Branch(LibGit2Sharp.Branch branch)
    {
        Id = branch.CanonicalName;
        Name = branch.FriendlyName;
        Commits = branch.Commits.Select(c => new Commit(c));
    }
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable Commits { get; set; }
}