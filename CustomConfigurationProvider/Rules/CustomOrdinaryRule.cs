namespace CustomConfigurationProvider.Rules;

public abstract class CustomOrdinaryRule : ICustomConfigurationRule
{
    public void Apply(IDictionary<string, string?> data, string sectionName, string? value)
    {
        data[sectionName] = GetValue(value);
    }

    protected abstract string? GetValue(string? value);

    public abstract bool Check(IDictionary<string, string?> data, string sectionName, string? value);
}