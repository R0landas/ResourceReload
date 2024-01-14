using MediatR;
using ResourceReload.Core.Common.Interfaces;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.Features.CopyResourceFiles;
using ResourceReload.Core.Features.ResourceManagement;
using Stateless;

namespace ResourceReload.Core.ResourceWatcherStateMachine;

public class ResourceWatcher
{
    private readonly ResourceConfig config;
    private readonly IMediator mediator;
    private readonly ILogger logger;
    
    private readonly StateMachine<ResourceWatcherState, ResourceWatcherTrigger> machine = new (ResourceWatcherState.Initial);

    public ResourceWatcher(IMediator mediator, ResourceConfig config, ILogger logger)
    {
        this.mediator = mediator;
        this.config = config;
        this.logger = logger;

        machine.Configure(ResourceWatcherState.Initial)
            .Permit(ResourceWatcherTrigger.Started, ResourceWatcherState.WatchingForChanges);

        machine.Configure(ResourceWatcherState.WatchingForChanges)
            .OnEntry(() => logger.LogInformation($"Watching for {config.Name} changes..."))
            .Permit(ResourceWatcherTrigger.Changed, ResourceWatcherState.WaitingForResourceToStop);

        machine.Configure(ResourceWatcherState.WaitingForResourceToStop)
            .OnEntryAsync(() => mediator.Send(new StopResourceCommand(config.Name)))
            .Permit(ResourceWatcherTrigger.Stopped, ResourceWatcherState.CopyingResource)
            .Ignore(ResourceWatcherTrigger.Changed);

        machine.Configure(ResourceWatcherState.CopyingResource)
            .OnEntryAsync(() => mediator.Send(new CopyResourceFilesCommand(config)))
            .Permit(ResourceWatcherTrigger.FilesCopied, ResourceWatcherState.StartingResource);

        machine.Configure(ResourceWatcherState.StartingResource)
            .OnEntry(() => logger.LogInformation($"Restarting resource {config.Name}"))
            .OnEntryAsync(() => mediator.Send(new StartResourceCommand(config.Name)))
            .Permit(ResourceWatcherTrigger.Started, ResourceWatcherState.WatchingForChanges);
    }

    public void Fire(ResourceWatcherTrigger trigger)
    {
        machine.Fire(trigger);
    }

    public Task FireAsync(ResourceWatcherTrigger trigger)
    {
        return machine.FireAsync(trigger);
    }
} 