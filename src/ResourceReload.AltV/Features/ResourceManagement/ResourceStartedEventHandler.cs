using AltV.Net;
using AltV.Net.Async;
using MediatR;
using ResourceReload.AltV.Common.Interfaces;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.Features.ResourceManagement;

namespace ResourceReload.AltV.Features.ResourceManagement;

internal sealed class ResourceStartedEventHandler(IPublisher mediator, ResourceReloadConfig config) : IEventHandler
{
    public void Register()
    {
        AltAsync.OnResourceStart += ResourceStarted;
    }

    public void Unregister()
    {
        AltAsync.OnResourceStart -= ResourceStarted;
    }

    private async Task ResourceStarted(INativeResource resource)
    {
        if (!config.Resources.Any(r => r.Name.Equals(resource.Name)))
        {
            return;
        }
        
        await mediator.Publish(new ResourceStartedNotification(resource.Name));
    }
}