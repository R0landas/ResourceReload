namespace ResourceReload.Core.ResourceWatcherStateMachine;

public enum ResourceWatcherTrigger
{
    Started,
    Changed,
    Stopped,
    FilesCopied,
}