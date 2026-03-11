using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CustomConfigurationProvider.Rules;

public abstract class CustomArrayRule : ICustomConfigurationRule
{
    private const string firstSectionIndexKey = ":0";

    public virtual void Apply(IDictionary<string, string?> data, string sectionName, string? value)
    {
        var strings = GetArray(value);

        data.Remove(sectionName);
        
        var arraySectionPrefix = GetArraySectionPrefix(sectionName);
        var arrayCount = GetArrayCount(data, arraySectionPrefix);
        var arraySectionName = arrayCount > 1 ? sectionName : arraySectionPrefix;

        SetArray(data, arraySectionName, strings);
    }

    private static int GetArrayCount(IDictionary<string, string?> data, string arraySectionPrefix)
    {
        return data.Keys
        .Where(k => k.StartsWith(arraySectionPrefix))
        .Select(k => {
            var remaining = k.Substring(arraySectionPrefix.Length);
            int indexOfColon = remaining.IndexOf(':');
            return indexOfColon == -1 ? remaining : remaining.Substring(0, indexOfColon);
        })
        .Distinct()
        .Count();
    }

    private static string GetArraySectionPrefix(string sectionName)
    {
        int place = sectionName.LastIndexOf(firstSectionIndexKey);

        if (place == -1)
            return sectionName; 

        return sectionName.Remove(place, firstSectionIndexKey.Length).Insert(place, "");
    }

    public bool Check(IDictionary<string, string?> data, string sectionName, string? value)
    {
        return SectionIsArray(sectionName) && CheckSection(data, sectionName, value);
    }

    protected abstract bool CheckSection(IDictionary<string, string?> data, string sectionName, string? value);

    protected virtual bool SectionIsArray(string sectionName)
    {
        return sectionName.Contains(firstSectionIndexKey);
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