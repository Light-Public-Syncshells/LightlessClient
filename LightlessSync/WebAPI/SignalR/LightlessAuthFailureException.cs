namespace LightlessSync.WebAPI.SignalR;

public class LightlessAuthFailureException : Exception
{
    public LightlessAuthFailureException(string reason)
    {
        Reason = reason;
    }

    public string Reason { get; }
}