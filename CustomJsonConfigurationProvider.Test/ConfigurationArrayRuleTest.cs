using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CustomJsonConfigurationProvider.Test;

public class ConfigurationArrayRuleTest
{
    [Fact]
    public void ApplyRuleWhenAddedToAppSettingsSuccess()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.SetAppSettingsCustomJsonConfigurationProvider();

        var strings = new string[] { "test1", "test2"};
        builder.Configuration.AddCustomConfigurationRule<CustomJsonConfigurationSource>(new TestArrayRule(strings));

        Assert.Equal(strings, builder.Configuration.GetSection("Test:TestSection").Get<string[]>());
    }

    [Fact]
    public void ApplyRuleWhenAddedCustomSourceSuccess()
    {
        var builder = Host.CreateApplicationBuilder();
        var source = builder.Configuration.AddCustomJsonConfigurationProvider("customsettings.json");
        var strings = new string[] { "test1", "test2" };
        source.AddCustomConfigurationRule(new TestArrayRule(strings));

        Assert.Equal(strings, builder.Configuration.GetSection("TestCustom:TestCustomSection").Get<string[]>());
    }
}
