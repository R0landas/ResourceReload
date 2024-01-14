using AltV.Net;
using AltV.Net.Async;
using MediatR;
using ResourceReload.AltV.Common.Interfaces;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.Features.ResourceManagement;

namespace ResourceReload.AltV.Features.ResourceManagement;

internal sealed class ResourceStoppedEventHandler(IPublisher mediator, ResourceReloadConfig config) : IEventHandler
{

    public void Register()
    {
        AltAsync.OnResourceStop += AltAsyncOnOnResourceStop;
    }

    public void Unregister()
    {
        AltAsync.OnResourceStop -= AltAsyncOnOnResourceStop;
    }

    private async Task AltAsyncOnOnResourceStop(INativeResource resource)
    {
        // When stopping the server, resources seem to be destroyed before the event is called
        // thus trying to access resource.Name later on causes an exception to be thrown
        if (ResourceReload.ResourceCancellationToken.IsCancellationRequested)
        {
            return;
        }
        
        if (!config.Resources.Any(r => r.Name.Equals(resource.Name)))
        {
            return;
        }
        
        await mediator.Publish(new ResourceStoppedNotification(resource.Name));
    }
}