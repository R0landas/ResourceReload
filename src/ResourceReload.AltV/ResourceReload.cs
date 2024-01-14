using AltV.Net;
using AltV.Net.Async;
using Microsoft.Extensions.DependencyInjection;
using ResourceReload.AltV.Common.Interfaces;
using ResourceReload.AltV.Features.Configuration;
using ResourceReload.Core.Features.Configuration;

namespace ResourceReload.AltV;

public class ResourceReload : AsyncResource
{
    /// <summary>
    /// When stopping the server, the AltAsync.OnResourceStop event is invoked.
    /// Sometimes resources are destroyed before the invocation of the event
    /// and doing any logic in the event handler leads to exceptions
    /// thus we need a way to know when we should ignore it
    /// </summary>
    public static CancellationToken ResourceCancellationToken => Cts.Token;
    private static readonly CancellationTokenSource Cts = new();

    private ServiceProvider? serviceProvider;
    
    public override void OnStart()
    {
        var version = typeof(ResourceReload).Assembly.GetName().Version;
        Alt.LogInfo($"Initializing Resource Reload {version} @R0landas");
        Alt.LogInfo("To download updates, go to https://github.com/R0landas/ResourceReload/releases");

        var config = ReadConfiguration();
        
        serviceProvider = new ServiceCollection()
            .RegisterServices(config)
            .BuildServiceProvider();
        
        serviceProvider.StartServices();
    }

    public override void OnStop()
    {
        Cts.Cancel();
        var eventHandlers = serviceProvider?.GetServices<IEventHandler>();

        foreach (var eventHandler in eventHandlers ?? [])
        {
            eventHandler.Unregister();
        }

        serviceProvider?.Dispose();

        Alt.Log("Resource Reload stopped");
    }

    private static ResourceReloadConfig ReadConfiguration()
    {
        var configurationReader = new ConfigurationReader();
        var configuration = configurationReader.GetConfiguration();

        if (configuration is null)
        {
            Alt.LogError("Failed reading Resource Reload config");
            throw new ApplicationException("Failed reading Resource Reload config");
        }

        return configuration;
    }
}