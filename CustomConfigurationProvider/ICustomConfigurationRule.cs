namespace CustomConfigurationProvider;

public interface ICustomConfigurationRule
{
    bool Check(string value);

    string TransformValue(string value);
}