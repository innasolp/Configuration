using Microsoft.Extensions.Configuration;

namespace CustomConfigurationProvider;

public interface ICustomConfigurationSource : IConfigurationSource
{
    IList<ICustomConfigurationRule> Rules { get; }
}