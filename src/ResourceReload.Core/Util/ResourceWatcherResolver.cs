using Microsoft.Extensions.DependencyInjection;
using ResourceReload.Core.ResourceWatcherStateMachine;

namespace ResourceReload.Core.Util;

internal sealed class ResourceWatcherResolver(IServiceProvider provider) : IResourceWatcherResolver
{
    public ResourceWatcher Resolve(string resourceName)
    {
        return provider.GetRequiredKeyedService<ResourceWatcher>(resourceName);
    }
}