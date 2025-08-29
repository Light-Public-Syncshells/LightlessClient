using LightlessSync.LightlessConfiguration.Configurations;

namespace LightlessSync.LightlessConfiguration;

public class ServerTagConfigService : ConfigurationServiceBase<ServerTagConfig>
{
    public const string ConfigName = "servertags.json";

    public ServerTagConfigService(string configDir) : base(configDir)
    {
    }

    public override string ConfigurationName => ConfigName;
}