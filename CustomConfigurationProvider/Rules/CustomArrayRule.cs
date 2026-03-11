namespace CustomConfigurationProvider.Rules;

public abstract class CustomArrayRule : ICustomConfigurationRule
{
    private const string sectionIndex = ":0";

    public virtual void Apply(IDictionary<string, string?> data, string sectionName, string? value)
    {
        var strings = GetArray(value);

        data.Remove(sectionName);
        
        var arraySectionName = GetArraySectionName(sectionName);

        SetArray(data, arraySectionName, strings);
    }

    private static string GetArraySectionName(string sectionName)
    {
        int place = sectionName.LastIndexOf(sectionIndex);

        if (place == -1)
            return sectionName; 

        return sectionName.Remove(place, sectionIndex.Length).Insert(place, "");
    }

    public bool Check(IDictionary<string, string?> data, string sectionName, string? value)
    {
        return SectionIsArray(sectionName) && CheckSection(data, sectionName, value);
    }

    protected abstract bool CheckSection(IDictionary<string, string?> data, string sectionName, string? value);

    protected virtual bool SectionIsArray(string sectionName)
    {
        return sectionName.Contains(sectionIndex);
    }

    protected abstract string[] GetArray(string? value);

    protected virtual void SetArray(IDictionary<string, string?> data, string arraySectionName, string[] strings )
    {
        for (int i = 0; i < strings.Length; i++)
        {
            data[$"{arraySectionName}:{i}"] = strings[i];
        }
    }
}