namespace CustomConfigurationProvider.Rules;

public abstract class CustomArrayRule : ICustomConfigurationRule
{
    private const string sectionIndex = ":0";

    public virtual void Apply(IDictionary<string, string?> data, string sectionName, string? value)
    {
        var strings = GetArray(value);

        data.Remove(sectionName);
        
        var arraySectionName = sectionName.EndsWith(sectionIndex) ? sectionName.Replace(sectionIndex, "") : sectionName;

        SetArray(data, arraySectionName, strings);
    }

    public abstract bool Check(IDictionary<string, string?> data, string sectionName, string? value);

    protected abstract string[] GetArray(string? value);

    protected virtual void SetArray(IDictionary<string, string?> data, string arraySectionName, string[] strings )
    {
        for (int i = 0; i < strings.Length; i++)
        {
            data[$"{arraySectionName}:{i}"] = strings[i];
        }
    }
}