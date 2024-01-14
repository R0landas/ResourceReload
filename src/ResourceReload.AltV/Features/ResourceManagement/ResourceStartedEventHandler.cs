using AltV.Net;
using MediatR;
using ResourceReload.AltV.Common.Interfaces;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.Features.ResourceManagement;

namespace ResourceReload.AltV.Features.ResourceManagement;

internal sealed class ResourceStartedEventHandler(IPublisher mediator, ResourceReloadConfig config) : IEventHandler
{
    public void Register()
    {
        Alt.OnAnyResourceStart += ResourceStarted;
    }

    public void Unregister()
    {
        Alt.OnAnyResourceStart -= ResourceStarted;
    }

    private async void ResourceStarted(INativeResource resource)
    {
        if (!config.Resources.Any(r => r.Name.Equals(resource.Name)))
        {
            return;
        }
        
        await mediator.Publish(new ResourceStartedNotification(resource.Name));
    }
}