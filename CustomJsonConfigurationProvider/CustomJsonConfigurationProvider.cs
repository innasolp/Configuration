using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration.Json;

namespace CustomJsonConfigurationProvider;

public class CustomJsonConfigurationProvider : JsonConfigurationProvider, ICustomConfigurationProvider
{
    private readonly CustomJsonConfigurationSource _customJsonConfigurationSource;

    ICustomConfigurationSource ICustomConfigurationProvider.CustomConfigurationSource => _customJsonConfigurationSource;

    public CustomJsonConfigurationProvider(CustomJsonConfigurationSource source) : base(source)
    {
        if(Source is not CustomJsonConfigurationSource customJsonConfigurationSource)
            throw new ArgumentException($"Source must be of type {typeof(CustomJsonConfigurationSource).FullName}, but was {Source?.GetType().FullName ?? "null"}.",
                            nameof(source));

        _customJsonConfigurationSource = customJsonConfigurationSource;

        _customJsonConfigurationSource.Rules.CollectionChanged += Rules_CollectionChanged;
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

        foreach (var rule in _customJsonConfigurationSource.Rules)
            SetRule(rule);
    }

    private void SetRule(ICustomConfigurationRule rule)
    {
        foreach (var dataSection in Data.Where(d => d.Value is not null && rule.Check(d.Key, d.Value)).ToList())
            Data[dataSection.Key] = rule.TransformValue(dataSection.Value!);
    }

    protected override void Dispose(bool disposing)
    {
        _customJsonConfigurationSource.Rules.CollectionChanged -= Rules_CollectionChanged;

        base.Dispose(disposing);
    }
}