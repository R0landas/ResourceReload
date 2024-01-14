using MediatR;

namespace ResourceReload.Core.Features.ResourceManagement;

public record ResourceStartedNotification(string ResourceName) : INotification;