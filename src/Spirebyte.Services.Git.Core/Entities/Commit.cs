using System;

namespace Spirebyte.Services.Git.Core.Entities;

public class Commit
{
    public Commit(string id, string sha, string shortMessage, string message, string authorUsername, string committerUsername, DateTime createdAt)
    {
        Id = id;
        Sha = sha;
        ShortMessage = shortMessage;
        Message = message;
        AuthorUsername = authorUsername;
        CommitterUsername = committerUsername;
        CreatedAt = createdAt;
    }
    
    public Commit(LibGit2Sharp.Commit commit)
    {
        Id = commit.Id.ToString(7);
        Sha = commit.Sha;
        ShortMessage = commit.MessageShort;
        Message = commit.Message;
        AuthorUsername = commit.Author.Name;
        CommitterUsername = commit.Committer.Name;
        CreatedAt = commit.Author.When.DateTime;
    }
    public string Id { get; set; }
    public string Sha { get; set; }
    public string ShortMessage { get; set; }
    public string Message { get; set; }
    public string AuthorUsername { get; set; }
    public string CommitterUsername { get; set; }
    public DateTime CreatedAt { get; set; }
}