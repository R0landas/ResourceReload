using MediatR;
using ResourceReload.Core.Common.Interfaces;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.Features.WatchResourceBuildDirectory.Notifications;

namespace ResourceReload.Core.Features.WatchResourceBuildDirectory;

public class BuildDirectoryWatcherDaemon(IPublisher mediator, ILogger logger) : IDisposable
{
    private IEnumerable<FileSystemWatcher> fileSystemWatchers = [];
    private ResourceConfig? resource;
    private DateTime timeSinceLastUpdate = DateTime.UtcNow;

    public void Initialize(ResourceConfig resourceConfig)
    {
        logger.LogInformation($"Resource Watcher set up for resource {resourceConfig.Name}");
        resource = resourceConfig;
        
        fileSystemWatchers = resourceConfig.Projects.Select(config =>
        {
            var fileSystemWatcher = new FileSystemWatcher(config.BuildDirectory)
            {
                NotifyFilter = NotifyFilters.LastWrite,
                IncludeSubdirectories = true,
                Filter = "*.*",
                EnableRaisingEvents = true,
            };

            fileSystemWatcher.Changed += async (_, _) => await PublishFilesChangedNotification();
            
            return fileSystemWatcher;
        }).ToList();
    }

    public void Dispose()
    {
        foreach (var fileSystemWatcher in fileSystemWatchers)
        {
            fileSystemWatcher.Dispose();
        }
        
        GC.SuppressFinalize(this);
    }

    private async Task PublishFilesChangedNotification()
    {
        if (resource is null)
        {
            throw new InvalidOperationException($"{nameof(resource)} is not set, but files changed event was invoked");
        }

        if (DateTime.UtcNow.Subtract(timeSinceLastUpdate) < TimeSpan.FromSeconds(2))
        {
            return;
        }
        
        await mediator.Publish(new ResourceBuildDirectoryChangedNotification(resource.Name));
        timeSinceLastUpdate = DateTime.UtcNow;
    }
}