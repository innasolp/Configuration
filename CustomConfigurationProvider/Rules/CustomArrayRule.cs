namespace CustomConfigurationProvider.Rules;

public abstract class CustomArrayRule : ICustomConfigurationRule
{
    private const string firstSectionIndexKey = ":0";

    private static string GetArraySectionPrefix(string sectionName)
    {
        int place = sectionName.LastIndexOf(firstSectionIndexKey);

        if (place == -1)
            return sectionName;

        return sectionName.Remove(place, firstSectionIndexKey.Length).Insert(place, "");
    }

    public virtual void Apply(IDictionary<string, string?> data, string sectionName, string? value)
    {
        var strings = GetArray(value);

        data.Remove(sectionName);
        
        var arraySectionPrefix = GetArraySectionPrefix(sectionName);
        
        SetArray(data, arraySectionPrefix, strings);
    }

    protected abstract string[] GetArray(string? value);

    protected virtual void SetArray(IDictionary<string, string?> data, string arraySectionName, string[] strings)
    {
        for (int i = 0; i < strings.Length; i++)
        {
            data[$"{arraySectionName}:{i}"] = strings[i];
        }
    }

    public bool Check(IDictionary<string, string?> data, string sectionName, string? value)
    {
        return SectionIsSingleElementArray(data, sectionName) && CheckSection(data, sectionName, value);
    }    

    protected abstract bool CheckSection(IDictionary<string, string?> data, string sectionName, string? value);

    protected virtual bool SectionIsSingleElementArray(IDictionary<string, string?> data, string sectionName)
    {
        var arraySectionPrefix = GetArraySectionPrefix(sectionName);

        return sectionName.Contains(firstSectionIndexKey) && GetArrayCount(data, arraySectionPrefix) == 1;
    }

    private static int GetArrayCount(IDictionary<string, string?> data, string arraySectionPrefix)
    {
        return data.Keys
        .Where(k => k.StartsWith(arraySectionPrefix))
        .Select(k => {
            var remaining = k.Substring(arraySectionPrefix.Length);
            int indexOfColon = remaining.IndexOf(':');
            return indexOfColon == -1 ? remaining : remaining.Substring(indexOfColon);
        })
        .Distinct()
        .Count();
    }
}