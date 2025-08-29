using LightlessSync.LightlessConfiguration.Configurations;

namespace LightlessSync.LightlessConfiguration;

public class PlayerPerformanceConfigService : ConfigurationServiceBase<PlayerPerformanceConfig>
{
    public const string ConfigName = "playerperformance.json";
    public PlayerPerformanceConfigService(string configDir) : base(configDir) { }

    public override string ConfigurationName => ConfigName;
}