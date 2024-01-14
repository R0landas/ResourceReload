using AltV.Net;
using AltV.Net.ColoredConsole;
using ResourceReload.Core.Common.Interfaces;

namespace ResourceReload.AltV;

internal sealed class AltVLogger : ILogger
{
    private const string LogPrefix = "~m~[Resource Reload]~w~ ";
    
    public void LogInformation(string message)
    {
        LogColored(message);
    }

    public void LogError(string message)
    {
        Alt.LogError(LogPrefix + message);
    }

    private void LogColored(string message)
    {
        Alt.LogColored(new ColoredMessage() + LogPrefix + message);
    }
}