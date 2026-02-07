using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration.Json;
namespace CustomJsonConfigurationProvider;

public class CustomJsonConfigurationProvider : JsonConfigurationProvider, ICustomConfigurationProvider
{
    private CustomJsonConfigurationSource CustomJsonConfigurationSource => (Source as CustomJsonConfigurationSource)!;

    ICustomConfigurationSource ICustomConfigurationProvider.CustomConfigurationSource => CustomJsonConfigurationSource;

    public CustomJsonConfigurationProvider(CustomJsonConfigurationSource source) : base(source)
    {
        if(Source is not CustomJsonConfigurationSource customJsonConfigurationSource)
            throw new ArgumentException("Source must be of type CustomJsonConfigurationSource");

        customJsonConfigurationSource.Rules.CollectionChanged += Rules_CollectionChanged;
    }

    private void Rules_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems?.Count > 0)
        {
            foreach (var rule in e.NewItems.OfType<ICustomConfigurationRule>())
                SetRule(rule);
        }
    }

    public override void Load()
    {
        base.Load();

        if (CustomJsonConfigurationSource == null) return;

        foreach (var rule in CustomJsonConfigurationSource.Rules)
            SetRule(rule);
    }

    private void SetRule(ICustomConfigurationRule rule)
    {
        foreach (var dataSection in Data.Where(d => d.Value is not null && rule.Check(d.Key, d.Value)))
            Data[dataSection.Key] = rule.TransformValue(dataSection.Value!);
    }
}