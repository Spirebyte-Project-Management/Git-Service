using Bogus;
using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Tests.Shared.MockData.Entities;

public sealed class BranchFaker : Faker<Branch>
{
    public static BranchFaker Instance => new();

    private BranchFaker()
    {
        CustomInstantiator(_ => new Branch(default, default, default, default));
        RuleFor(r => r.Id, f => f.Random.Hash(7));
        RuleFor(r => r.Name, f => f.Random.Word());
        RuleFor(r => r.Commits, f => CommitFaker.Instance.Generate(f.Random.Number(1, 20)));
    }
}