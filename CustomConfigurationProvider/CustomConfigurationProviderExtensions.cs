using Microsoft.Extensions.Configuration;

namespace CustomConfigurationProvider;

public static class CustomConfigurationProviderExtensions
{
    public static IConfigurationBuilder AddCustomConfigurationRule<TConfigurationSource>(this IConfigurationBuilder builder, ICustomConfigurationRule customConfigurationRule)
        where TConfigurationSource : ICustomConfigurationSource
    {
        var customJsonConfigurationSource = builder.Sources.OfType<TConfigurationSource>().FirstOrDefault();

        customJsonConfigurationSource?.Rules.Add(customConfigurationRule);

        return builder;
    }

    public static IConfigurationBuilder AddCustomConfigurationRule(this IConfigurationBuilder builder, ICustomConfigurationRule customConfigurationRule)
    {
        var customJsonConfigurationSource = builder.Sources.OfType<ICustomConfigurationSource>().FirstOrDefault();

        customJsonConfigurationSource?.Rules.Add(customConfigurationRule);

        return builder;
    }

    public static IConfigurationBuilder AddCustomConfigurationRule<TConfigurationSource, TRule>(this IConfigurationBuilder builder)
        where TRule : ICustomConfigurationRule, new()
        where TConfigurationSource : FileConfigurationSource, ICustomConfigurationSource
    {
        var customJsonConfigurationSource = builder.Sources.OfType<TConfigurationSource>().FirstOrDefault();

        customJsonConfigurationSource?.Rules.Add(new TRule());

        return builder;
    }
}