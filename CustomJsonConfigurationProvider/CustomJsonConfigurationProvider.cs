using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration.Json;

namespace CustomJsonConfigurationProvider;

public class CustomJsonConfigurationProvider : JsonConfigurationProvider, ICustomConfigurationProvider
{
    private readonly Lock _loadLock = new();

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
        lock (_loadLock)
        {
            base.Load();

            foreach (var rule in _customJsonConfigurationSource.Rules)
                SetRule(rule);
        }
    }

    private void SetRule(ICustomConfigurationRule rule)
    {
        var data = new Dictionary<string, string?>(Data);

        foreach (var dataSection in data.Where(d => d.Value is not null && rule.Check(Data, d.Key, d.Value)).ToList())
        {
            rule.Apply(data, dataSection.Key, dataSection.Value);
        }

        ApplyData(data);
    }

    protected virtual void ApplyData(Dictionary<string, string?> newData)
    {
        Data = newData;
    }

    protected override void Dispose(bool disposing)
    {
        _customJsonConfigurationSource.Rules.CollectionChanged -= Rules_CollectionChanged;

        base.Dispose(disposing);
    }
}