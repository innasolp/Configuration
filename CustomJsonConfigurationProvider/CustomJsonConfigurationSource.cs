using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.ObjectModel;


namespace CustomJsonConfigurationProvider;

public class CustomJsonConfigurationSource : JsonConfigurationSource, ICustomConfigurationSource
{
    private readonly ObservableCollection<ICustomConfigurationRule> _rules = [];

    internal ObservableCollection<ICustomConfigurationRule> Rules => _rules;

    IList<ICustomConfigurationRule> ICustomConfigurationSource.Rules => Rules;

    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new CustomJsonConfigurationProvider(this);
    }
}