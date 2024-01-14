using ResourceReload.Core.ResourceWatcherStateMachine;

namespace ResourceReload.Core.Util;

public interface IResourceWatcherResolver
{
    ResourceWatcher Resolve(string resourceName);
}