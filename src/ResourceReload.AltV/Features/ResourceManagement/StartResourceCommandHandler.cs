using AltV.Net;
using AltV.Net.Async;
using MediatR;
using ResourceReload.Core.Features.ResourceManagement;

namespace ResourceReload.AltV.Features.ResourceManagement;

internal sealed class StartResourceCommandHandler : IRequestHandler<StartResourceCommand>
{
    public Task Handle(StartResourceCommand request, CancellationToken cancellationToken)
    {
        AltAsync.RunOnMainThread(() => Alt.StartResource(request.ResourceName));
        
        return Task.CompletedTask;
    }
}