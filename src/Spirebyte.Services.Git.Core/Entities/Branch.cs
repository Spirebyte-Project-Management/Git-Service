using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Git.Core.Entities;

public class Branch
{
    public Branch(string id, string name, bool isHead, List<Commit> commits)
    {
        Id = id;
        Name = name;
        IsHead = isHead;
        Commits = commits;
    }
    public Branch(LibGit2Sharp.Branch branch)
    {
        Id = branch.CanonicalName;
        Name = branch.FriendlyName;
        IsHead = branch.IsCurrentRepositoryHead;
        Commits = branch.Commits.Select(c => new Commit(c)).ToList();
    }
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsHead { get; set; }
    public List<Commit> Commits { get; set; }
}