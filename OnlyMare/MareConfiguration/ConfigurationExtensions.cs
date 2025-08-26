﻿using OnlyMare.OnlyMareConfiguration.Configurations;

namespace OnlyMare.OnlyMareConfiguration;

public static class ConfigurationExtensions
{
    public static bool HasValidSetup(this OnlyMareConfig configuration)
    {
        return configuration.AcceptedAgreement && configuration.InitialScanComplete
                    && !string.IsNullOrEmpty(configuration.CacheFolder)
                    && Directory.Exists(configuration.CacheFolder);
    }
}