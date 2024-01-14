using AltV.Net;
using AltV.Net.Async;
using ResourceReload.AltV.Common.Interfaces;

namespace ResourceReload.AltV.Features.AutoReconnect;

internal sealed class ReconnectPlayerOnResourceStartedEventHandler(ReconnectService reconnectService) : IEventHandler
{
    public void Register()
    {
        AltAsync.OnResourceStart += ResourceStarted;
    }

    public void Unregister()
    {
        AltAsync.OnResourceStart -= ResourceStarted;
    }
    
    private Task ResourceStarted(INativeResource resource)
    {
        return reconnectService.ConnectLocalClient();
    }
}