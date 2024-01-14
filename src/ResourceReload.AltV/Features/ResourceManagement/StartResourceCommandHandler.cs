using AltV.Net;
using MediatR;
using ResourceReload.Core.Features.ResourceManagement;

namespace ResourceReload.AltV.Features.ResourceManagement;

internal sealed class StartResourceCommandHandler : IRequestHandler<StartResourceCommand>
{
    public Task Handle(StartResourceCommand request, CancellationToken cancellationToken)
    {
        Alt.StartResource(request.ResourceName);
        
        return Task.CompletedTask;
    }
}