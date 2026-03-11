using CustomJsonConfigurationProvider.Rules;
using System.Text.Json;

namespace CustomJsonConfigurationProvider.Test;

internal class TestJsonObjectRule(JsonDocument jsonDoc) : CustomJsonObjectRule
{
    public override bool Check(IDictionary<string, string?> data, string sectionName, string? value) => 
        value?.StartsWith("${") == true && value?.EndsWith("}") == true;

    protected override JsonDocument CreateJson(IDictionary<string, string?> data, string sectionName, string? value)
    {
        return JsonDocument.Parse(jsonDoc.RootElement.GetRawText());
    }
}