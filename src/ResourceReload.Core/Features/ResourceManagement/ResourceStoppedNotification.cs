using MediatR;

namespace ResourceReload.Core.Features.ResourceManagement;

public record ResourceStoppedNotification(string ResourceName) : INotification;