using LightlessSync.LightlessConfiguration.Models;

namespace LightlessSync.LightlessConfiguration.Configurations;

public class UidNotesConfig : ILightlessConfiguration
{
    public Dictionary<string, ServerNotesStorage> ServerNotes { get; set; } = new(StringComparer.Ordinal);
    public int Version { get; set; } = 0;
}
