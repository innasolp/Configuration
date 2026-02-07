using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CustomJsonConfigurationProvider;

public static class CustomJsonConfigurationProviderExtensions
{
    private const string DefaultConfigurationFileName = "appsettings.json";

    public static IConfigurationBuilder SetAppSettingsCustomJsonConfigurationProvider(this IConfigurationBuilder builder)
    {
        var providers = builder.Sources.Where(s => s is JsonConfigurationSource jsonConfigSource
                && string.Equals(jsonConfigSource.Path, DefaultConfigurationFileName, StringComparison.OrdinalIgnoreCase)).ToList();

        foreach (var provider in providers)
            builder.Sources.Remove(provider);

        return builder.Add(new CustomJsonConfigurationSource() { Path = DefaultConfigurationFileName });
    }

    public static ICustomConfigurationSource AddCustomJsonConfigurationProvider(this IConfigurationBuilder builder, string fileName)
    {
        if(string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName));

        var source = new CustomJsonConfigurationSource() { Path = fileName };
        builder.Add(source);
        return source;
    }
}