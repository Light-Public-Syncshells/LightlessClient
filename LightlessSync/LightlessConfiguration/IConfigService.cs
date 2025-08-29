using LightlessSync.LightlessConfiguration.Configurations;

namespace LightlessSync.LightlessConfiguration;

public interface IConfigService<out T> : IDisposable where T : ILightlessConfiguration
{
    T Current { get; }
    string ConfigurationName { get; }
    string ConfigurationPath { get; }
    public event EventHandler? ConfigSave;
    void UpdateLastWriteTime();
}
