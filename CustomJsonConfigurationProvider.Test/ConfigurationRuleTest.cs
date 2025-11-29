using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CustomJsonConfigurationProvider.Test;

public class ConfigurationRuleTest
{
    [Fact]
    public void ApplyRuleWhenAddedToAppSettingsSuccess()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.SetAppSettingsCustomJsonConfigurationProvider();
        builder.Configuration.AddCustomConfigurationRule<CustomJsonConfigurationSource, TestRule>();

        Assert.Equal("TestValueApp", builder.Configuration.GetSection("Test:TestSection").Get<string>());
    }

    [Fact]
    public void ApplyRuleWhenAddedCustomSourceSuccess()
    {
        var builder = Host.CreateApplicationBuilder();
        var source = builder.Configuration.AddCustomJsonConfigurationProvider("customsettings.json");
        source.AddCustomConfigurationRule<TestRule>();

        Assert.Equal("TestValueCustom", builder.Configuration.GetSection("TestCustom:TestCustomSection").Get<string>());
    }
}