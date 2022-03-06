using System.Runtime.Serialization;
using Bogus;
using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Tests.Shared.MockData.Entities;

public sealed class RepositoryFaker : Faker<Repository>
{
    public static RepositoryFaker Instance => new();

    private RepositoryFaker()
    {
        CustomInstantiator(_ => FormatterServices.GetUninitializedObject(typeof(Repository)) as Repository);
        RuleFor(r => r.Id, f => f.Random.Word());
        RuleFor(r => r.Title, f => f.Commerce.ProductName());
        RuleFor(r => r.Description, f => f.Commerce.ProductDescription());
        RuleFor(r => r.ProjectId, f => f.Random.Word());
        RuleFor(r => r.ReferenceId, f => f.Random.Guid());
        RuleFor(r => r.Branches, f => BranchFaker.Instance.Generate(f.Random.Number(1, 20)));
        RuleFor(r => r.CreatedAt, f => f.Date.Past());
    }
}