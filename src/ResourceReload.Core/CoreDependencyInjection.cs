using Microsoft.Extensions.DependencyInjection;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.Features.WatchResourceBuildDirectory;
using ResourceReload.Core.Util;

namespace ResourceReload.Core;

public static class CoreDependencyInjection
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddTransient<BuildDirectoryWatcherDaemon>();
        services.AddTransient<IResourceWatcherResolver, ResourceWatcherResolver>();
    }

    public static void StartCoreServices(this IServiceProvider serviceProvider)
    {
        var config = serviceProvider.GetRequiredService<ResourceReloadConfig>();

        foreach (var resourceConfig in config.Resources)
        {
            var daemon = serviceProvider.GetRequiredService<BuildDirectoryWatcherDaemon>();
            daemon.Initialize(resourceConfig);
        }
    }
}