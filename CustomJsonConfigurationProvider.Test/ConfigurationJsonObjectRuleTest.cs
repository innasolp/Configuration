using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace CustomJsonConfigurationProvider.Test;

public class ConfigurationJsonObjectRuleTest
{
    [Fact]
    public void ApplyRuleWhenAddedToAppSettingsSuccess()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.SetAppSettingsCustomJsonConfigurationProvider();

        var obj = new { Id = 1, Name = "Test1" };
        var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(obj));    

        builder.Configuration.AddCustomConfigurationRule<CustomJsonConfigurationSource>(new TestJsonObjectRule(jsonDoc));

        Assert.Equal(obj.Id, builder.Configuration.GetSection("Test:TestSection:Id").Get<int>());
        Assert.Equal(obj.Name, builder.Configuration.GetSection("Test:TestSection:Name").Get<string>());
    }

    [Fact]
    public void ApplyRuleWhenAddedCustomSourceSuccess()
    {
        var builder = Host.CreateApplicationBuilder();
        var source = builder.Configuration.AddCustomJsonConfigurationProvider("customsettings.json");

        var obj = new { Id = 1, Name = "Test1" };
        var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(obj));
        source.AddCustomConfigurationRule(new TestJsonObjectRule(jsonDoc));

        Assert.Equal(obj.Id, builder.Configuration.GetSection("TestCustom:TestCustomSection:Id").Get<int>());
        Assert.Equal(obj.Name, builder.Configuration.GetSection("TestCustom:TestCustomSection:Name").Get<string>());
    }
}
