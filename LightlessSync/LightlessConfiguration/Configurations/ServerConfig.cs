using LightlessSync.LightlessConfiguration.Models;
using LightlessSync.WebAPI;

namespace LightlessSync.LightlessConfiguration.Configurations;

[Serializable]
public class ServerConfig : ILightlessConfiguration
{
    public int CurrentServer { get; set; } = 0;

    public List<ServerStorage> ServerStorage { get; set; } = new()
    {
        { new ServerStorage() { ServerName = ApiController.MainServer, ServerUri = ApiController.MainServiceUri, UseOAuth2 = true } },
    };

    public bool SendCensusData { get; set; } = false;
    public bool ShownCensusPopup { get; set; } = false;

    public int Version { get; set; } = 2;
}