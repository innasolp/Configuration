using CustomConfigurationProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CustomJsonConfigurationProvider.Test;

public class ConfigurationRuleTest
{
    [Fact]
    public void ApplyRuleWhenAddedSuccess()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.SetAppSettingsCustomJsonConfigurationProvider();
        builder.Configuration.AddCustomConfigurationRule<CustomJsonConfigurationSource, TestRule>();

        Assert.Equal("TestValue", builder.Configuration.GetSection("TestSection").Get<string>());
    }
}