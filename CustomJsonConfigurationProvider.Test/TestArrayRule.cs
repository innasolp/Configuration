using CustomConfigurationProvider.Rules;

namespace CustomJsonConfigurationProvider.Test;

internal class TestArrayRule(string arraySectionName, string[] strings) : CustomArrayRule
{
    protected override bool CheckSection(IDictionary<string, string?> data, string sectionName, string? value)
    {
        return sectionName.Contains(arraySectionName) 
            && value?.StartsWith("${") == true && value?.EndsWith("}") == true;
    }

    protected override string[] GetArray(string? value)
    {
        return strings;
    }
}