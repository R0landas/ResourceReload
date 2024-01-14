using MediatR;

namespace ResourceReload.Core.Features.ResourceManagement;

public record StopResourceCommand(string ResourceName) : IRequest;