using MediatR;
using ResourceReload.Core.Common.Interfaces;
using ResourceReload.Core.ResourceWatcherStateMachine;
using ResourceReload.Core.Util;

namespace ResourceReload.Core.Features.WatchResourceBuildDirectory.Notifications;

internal sealed class ResourceBuildDirectoryChangedNotificationHandler(
    ILogger logger,
    IResourceWatcherResolver resourceWatcherResolver)
    : INotificationHandler<ResourceBuildDirectoryChangedNotification>
{
    public async Task Handle(ResourceBuildDirectoryChangedNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Resource ~m~{notification.ResourceName} ~c~changed");

        var resourceWatcher = resourceWatcherResolver.Resolve(notification.ResourceName);
        await resourceWatcher.FireAsync(ResourceWatcherTrigger.Changed);
    }
}