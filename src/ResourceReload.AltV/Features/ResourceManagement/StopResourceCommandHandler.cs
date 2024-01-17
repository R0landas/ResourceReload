using AltV.Net;
using AltV.Net.Async;
using MediatR;
using ResourceReload.Core.Features.ResourceManagement;

namespace ResourceReload.AltV.Features.ResourceManagement;

internal sealed class StopResourceCommandHandler : IRequestHandler<StopResourceCommand>
{
    public Task Handle(StopResourceCommand request, CancellationToken cancellationToken)
    {
        AltAsync.RunOnMainThread(() => Alt.StopResource(request.ResourceName));
        
        return Task.CompletedTask;
    }
}