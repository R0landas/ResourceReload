using MediatR;
using ResourceReload.Core.ResourceWatcherStateMachine;
using ResourceReload.Core.Util;

namespace ResourceReload.Core.Features.ResourceManagement;

internal sealed class ResourceStartedNotificationHandler(IResourceWatcherResolver resourceWatcherResolver)
    : INotificationHandler<ResourceStartedNotification>
{
    public Task Handle(ResourceStartedNotification notification, CancellationToken cancellationToken)
    {
        var watcher = resourceWatcherResolver.Resolve(notification.ResourceName);
        watcher.Fire(ResourceWatcherTrigger.Started);
        
        return Task.CompletedTask;
    }
}