namespace CustomConfigurationProvider;

public interface ICustomConfigurationRule
{
    bool Check(string sectionName, string value);

    string TransformValue(string value);
}