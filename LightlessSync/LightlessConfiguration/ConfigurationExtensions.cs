using LightlessSync.LightlessConfiguration.Configurations;

namespace LightlessSync.LightlessConfiguration;

public static class ConfigurationExtensions
{
    public static bool HasValidSetup(this LightlessConfig configuration)
    {
        return configuration.AcceptedAgreement && configuration.InitialScanComplete
                    && !string.IsNullOrEmpty(configuration.CacheFolder)
                    && Directory.Exists(configuration.CacheFolder);
    }
}