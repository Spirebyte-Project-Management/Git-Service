using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Spirebyte.Services.Git.Tests.Shared.Factories;

[ExcludeFromCodeCoverage]
public class SpirebyteApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    protected override IWebHostBuilder CreateWebHostBuilder()
    {
        return base.CreateWebHostBuilder().UseEnvironment("Tests");
    }
}