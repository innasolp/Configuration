using CustomConfigurationProvider.Rules;

namespace CustomJsonConfigurationProvider.Test;

internal class TestArrayRule(string[] strings) : CustomArrayRule
{
    public override bool Check(IDictionary<string, string?> data, string sectionName, string? value)
    {
        return value?.StartsWith("${") == true && value?.EndsWith("}") == true;
    }

    protected override string[] GetArray(string? value)
    {
        return strings;
    }
}