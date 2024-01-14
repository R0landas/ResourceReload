using MediatR;

namespace ResourceReload.Core.Features.WatchResourceBuildDirectory.Notifications;

public record ResourceBuildDirectoryChangedNotification(string ResourceName) : INotification;