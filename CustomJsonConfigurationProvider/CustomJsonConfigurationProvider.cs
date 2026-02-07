using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration.Json;

using CustomJsonSource = CustomJsonConfigurationProvider.CustomJsonConfigurationSource;

namespace CustomJsonConfigurationProvider;

public class CustomJsonConfigurationProvider : JsonConfigurationProvider, ICustomConfigurationProvider
{
    private CustomJsonSource CustomJsonConfigurationSource => (Source as CustomJsonSource)!;

    ICustomConfigurationSource ICustomConfigurationProvider.CustomConfigurationSource => CustomJsonConfigurationSource;

    public CustomJsonConfigurationProvider(CustomJsonSource source) : base(source)
    {
        if(Source is not CustomJsonSource)
            throw new ArgumentException("Source must be of type CustomJsonConfigurationSource");

        CustomJsonConfigurationSource.Rules.CollectionChanged += Rules_CollectionChanged;
    }

    private void Rules_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems?.Count > 0)
        {
            foreach (var rule in e.NewItems.OfType<ICustomConfigurationRule>())
                SetRule(rule);

            OnReload();
        }
    }

    public override void Load()
    {
        base.Load();

        foreach (var rule in CustomJsonConfigurationSource.Rules)
            SetRule(rule);
    }

    private void SetRule(ICustomConfigurationRule rule)
    {
        foreach (var dataSection in Data.Where(d => d.Value is not null && rule.Check(d.Key, d.Value)).ToList())
            Data[dataSection.Key] = rule.TransformValue(dataSection.Value!);
    }

    protected override void Dispose(bool disposing)
    {
        CustomJsonConfigurationSource.Rules.CollectionChanged -= Rules_CollectionChanged;

        base.Dispose(disposing);
    }
}