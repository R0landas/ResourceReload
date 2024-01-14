using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ResourceReload.AltV.Common.Interfaces;
using ResourceReload.AltV.Features.AutoReconnect;
using ResourceReload.AltV.Features.Configuration;
using ResourceReload.AltV.Features.ResourceManagement;
using ResourceReload.Core;
using ResourceReload.Core.Common.Interfaces;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.ResourceWatcherStateMachine;

namespace ResourceReload.AltV;

internal static class Setup
{
    internal static IServiceCollection RegisterServices(this IServiceCollection services, AltVResourceReloadConfig config)
    {
        services.AddSingleton<IEventHandler, ResourceStartedEventHandler>();
        services.AddSingleton<IEventHandler, ResourceStoppedEventHandler>();
        
        services.AddSingleton(config);
        services.AddSingleton<ResourceReloadConfig>(config);
        
        services.AddSingleton<ILogger, AltVLogger>();
        
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<ResourceReload>();
            configuration.RegisterServicesFromAssemblyContaining<ICoreAssemblyMarker>();
        });

        foreach (var resourceConfig in config.Resources)
        {
            services.AddKeyedSingleton<ResourceWatcher>(resourceConfig.Name,
                (provider, _) => new ResourceWatcher(provider.GetRequiredService<IMediator>(), resourceConfig, provider.GetRequiredService<ILogger>()));
        }
        
        services.AddCoreServices();

        if (config.UseAutoReconnect)
        {
            services.AddSingleton<ReconnectService>();
            services.AddSingleton<IEventHandler, ReconnectPlayerOnResourceStartedEventHandler>();
        }

        return services;
    }

    internal static void StartServices(this IServiceProvider serviceProvider)
    {
        var eventHandlers = serviceProvider.GetServices<IEventHandler>();
        foreach (var eventHandler in eventHandlers)
        {
            eventHandler.Register();
        }
        
        serviceProvider.StartCoreServices();
    }
}