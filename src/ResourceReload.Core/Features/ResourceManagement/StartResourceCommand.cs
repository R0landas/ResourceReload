using MediatR;

namespace ResourceReload.Core.Features.ResourceManagement;

public record StartResourceCommand(string ResourceName) : IRequest;