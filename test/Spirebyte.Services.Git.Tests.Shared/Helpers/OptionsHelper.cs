using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Spirebyte.Services.Git.Tests.Shared.Helpers;

[ExcludeFromCodeCoverage]
public class OptionsHelper
{
    public static TSettings GetOptions<TSettings>(string section, string settingsFileName = null)
        where TSettings : class, new()
    {
        settingsFileName ??= "appsettings.Tests.json";
        var configuration = new TSettings();

        GetConfigurationRoot(settingsFileName)
            .GetSection(section)
            .Bind(configuration);

        return configuration;
    }

    private static IConfigurationRoot GetConfigurationRoot(string settingsFileName)
    {
        return new ConfigurationBuilder()
            .AddJsonFile(settingsFileName, true)
            .AddEnvironmentVariables()
            .Build();
    }
}