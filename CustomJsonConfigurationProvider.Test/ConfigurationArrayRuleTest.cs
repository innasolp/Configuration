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

        var testArraySection = "TestArraySection";
        var strings = new string[] { "test1", "test2"};
        builder.Configuration.AddCustomConfigurationRule<CustomJsonConfigurationSource>(new TestArrayRule(testArraySection, strings));

        Assert.Equal(strings, builder.Configuration.GetSection($"TestArray:{testArraySection}").Get<string[]>());
    }

    [Fact]
    public void ApplyRuleWhenAddedCustomSourceSuccess()
    {
        var builder = Host.CreateApplicationBuilder();
        var source = builder.Configuration.AddCustomJsonConfigurationProvider("customsettings.json");

        var testArraySection = "TestCustomArraySection"; 
        var strings = new string[] { "test1", "test2" };
        source.AddCustomConfigurationRule(new TestArrayRule(testArraySection, strings));

        Assert.Equal(strings, builder.Configuration.GetSection($"TestCustomArray:{testArraySection}").Get<string[]>());
    }
}
