namespace CustomConfigurationProvider;

public interface ICustomConfigurationRule
{
    bool Check(IDictionary<string, string?> data, string sectionName, string? value);

    void Apply(IDictionary<string, string?> data, string sectionName, string? value);
}