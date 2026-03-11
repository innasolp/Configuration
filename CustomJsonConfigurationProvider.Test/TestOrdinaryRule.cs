using CustomConfigurationProvider.Rules;

namespace CustomJsonConfigurationProvider.Test;

internal class TestOrdinaryRule : CustomOrdinaryRule
{
    public override bool Check(IDictionary<string, string?> data, string sectionName, string? value)
    {
        return value?.StartsWith("${") == true && value?.EndsWith("}") == true;
    }

    protected override string? GetValue(string? value)
    {
        return value?.Replace("${", "").Replace("}", "");
    }
}