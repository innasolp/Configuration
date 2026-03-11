using CustomConfigurationProvider;
using System.Text.Json;

namespace CustomJsonConfigurationProvider.Rules;

public abstract class CustomJsonObjectRule : ICustomConfigurationRule
{
    private const string sectionIndex = ":0";

    public abstract bool Check(IDictionary<string, string?> data, string sectionName, string? value);

    public virtual void Apply(IDictionary<string, string?> data, string sectionName, string? value)
    {
        using var jsonDoc = CreateJson(data, sectionName, value);

        data.Remove(sectionName);

        var objectSectionName = GetObjectSectionName(sectionName);

        try
        {
            var sectionsData = new Dictionary<string, string?>();

            FlattenElement(sectionsData, objectSectionName, jsonDoc.RootElement);

            foreach (var sectionData in sectionsData)
                data.Add(sectionData.Key, sectionData.Value);
        }
        catch
        {
            data[sectionName] = value;
        }
    }

    private static string GetObjectSectionName(string sectionName)
    {
        int place = sectionName.LastIndexOf(sectionIndex);

        if (place == -1)
            return sectionName;

        return sectionName.Remove(place, sectionIndex.Length).Insert(place, "");
    }

    protected abstract JsonDocument CreateJson(IDictionary<string, string?> data, string sectionName, string? value);

    protected virtual void FlattenElement(IDictionary<string, string?> data, string prefix, JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    FlattenElement(data, $"{prefix}:{property.Name}", property.Value);
                }
                break;

            case JsonValueKind.Array:
                int index = 0;
                foreach (var item in element.EnumerateArray())
                {
                    FlattenElement(data, $"{prefix}:{index++}", item);
                }
                break;

            case JsonValueKind.Null:
                data[prefix] = null;
                break;

            default:
                data[prefix] = element.ToString();
                break;
        }
    }
}