using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CustomJsonConfigurationProvider;

public static class CustomJsonConfigurationProviderExtensions
{
    public static IConfigurationBuilder SetAppSettingsCustomJsonConfigurationProvider(this IConfigurationBuilder builder)
    {
        var currentProvider = builder.Sources.FirstOrDefault(s => s is JsonConfigurationSource jsonConfigSource && jsonConfigSource.Path == "appsettings.json");
        if(currentProvider != null) builder.Sources.Remove(currentProvider);
        return builder.Add(new CustomJsonConfigurationSource() { Path = "appsettings.json"});
    }
}