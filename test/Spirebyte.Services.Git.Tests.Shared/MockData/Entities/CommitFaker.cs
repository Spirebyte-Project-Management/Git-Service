using Bogus;
using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Tests.Shared.MockData.Entities;

public sealed class CommitFaker : Faker<Commit>
{
    private CommitFaker()
    {
        CustomInstantiator(_ => new Commit(default, default, default, default,
            default, default, default));
        RuleFor(r => r.Id, f => f.Random.Hash(7));
        RuleFor(r => r.Sha, f => f.Random.Hash());
        RuleFor(r => r.Message, f => f.Random.Words());
        RuleFor(r => r.AuthorUsername, f => f.Internet.UserName());
        RuleFor(r => r.CommitterUsername, f => f.Internet.UserName());
        RuleFor(r => r.CreatedAt, f => f.Date.Past());
    }

    public static CommitFaker Instance => new();
}