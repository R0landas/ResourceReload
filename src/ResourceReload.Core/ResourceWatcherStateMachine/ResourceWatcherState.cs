namespace ResourceReload.Core.ResourceWatcherStateMachine;

public enum ResourceWatcherState
{
    Initial,
    WatchingForChanges,
    WaitingForResourceToStop,
    CopyingResource,
    StartingResource
}