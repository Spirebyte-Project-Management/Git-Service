using Spirebyte.Services.Git.API;
using Spirebyte.Services.Git.Tests.Shared.Factories;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]


namespace Spirebyte.Services.Git.Tests.Integration;

[CollectionDefinition("Spirebyte collection", DisableParallelization = true)]
public class SpirebyteCollection : ICollectionFixture<SpirebyteApplicationFactory<Program>>
{
}