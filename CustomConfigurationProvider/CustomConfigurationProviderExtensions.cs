using Microsoft.Extensions.Configuration;

namespace CustomConfigurationProvider;

public static class CustomConfigurationProviderExtensions
{
    public static IConfigurationBuilder AddCustomConfigurationRule<TConfigurationSource>(this IConfigurationBuilder builder, ICustomConfigurationRule customConfigurationRule)
        where TConfigurationSource : ICustomConfigurationSource
    {
        var customJsonConfigurationSources = builder.Sources.OfType<TConfigurationSource>();

        customJsonConfigurationSources.ToList().ForEach(s => s.Rules.Add(customConfigurationRule));

        return builder;
    }

    public static IConfigurationBuilder AddCustomConfigurationRule(this IConfigurationBuilder builder, ICustomConfigurationRule customConfigurationRule)
    {
        var customJsonConfigurationSources = builder.Sources.OfType<ICustomConfigurationSource>();

        customJsonConfigurationSources.ToList().ForEach(s => s.Rules.Add(customConfigurationRule));

        return builder;
    }

    public static IConfigurationBuilder AddCustomConfigurationRule<TConfigurationSource, TRule>(this IConfigurationBuilder builder)
        where TRule : ICustomConfigurationRule, new()
        where TConfigurationSource : FileConfigurationSource, ICustomConfigurationSource
    {
        var customJsonConfigurationSources = builder.Sources.OfType<TConfigurationSource>();

        customJsonConfigurationSources.ToList().ForEach(s => s.Rules.Add(new TRule()));

        return builder;
    }

    public static ICustomConfigurationSource AddCustomConfigurationRule(this ICustomConfigurationSource source, ICustomConfigurationRule customConfigurationRule)
    {
        source.Rules.Add(customConfigurationRule);

        return source;
    }

    public static ICustomConfigurationSource AddCustomConfigurationRule<TRule>(this ICustomConfigurationSource source)
        where TRule : ICustomConfigurationRule, new()       
    {
        source.Rules.Add(new TRule());

        return source;
    }
}