using AltV.Net;
using MediatR;
using ResourceReload.Core.Features.ResourceManagement;

namespace ResourceReload.AltV.Features.ResourceManagement;

internal sealed class StopResourceCommandHandler : IRequestHandler<StopResourceCommand>
{
    public Task Handle(StopResourceCommand request, CancellationToken cancellationToken)
    {
        Alt.StopResource(request.ResourceName);
        
        return Task.CompletedTask;
    }
}