using CustomConfigurationProvider;

namespace CustomJsonConfigurationProvider.Test;

internal class TestRule : ICustomConfigurationRule
{
    public bool Check(string sectionName, string value)
    {
        return value.StartsWith("${") && value.EndsWith("}");
    }

    public string TransformValue(string value)
    {
        return value.Replace("${", "").Replace("}", "");
    }
}
