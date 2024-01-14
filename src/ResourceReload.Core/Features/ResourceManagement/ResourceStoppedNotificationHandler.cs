using MediatR;
using ResourceReload.Core.ResourceWatcherStateMachine;
using ResourceReload.Core.Util;

namespace ResourceReload.Core.Features.ResourceManagement;

public class ResourceStoppedNotificationHandler(IResourceWatcherResolver resourceWatcherResolver)
    : INotificationHandler<ResourceStoppedNotification>
{
    public async Task Handle(ResourceStoppedNotification notification, CancellationToken cancellationToken)
    {
        var watcher = resourceWatcherResolver.Resolve(notification.ResourceName);

        await watcher.FireAsync(ResourceWatcherTrigger.Stopped);
    }
}