using MediatR;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.ResourceWatcherStateMachine;
using ResourceReload.Core.Util;

namespace ResourceReload.Core.Features.CopyResourceFiles;

internal sealed class CopyResourceFilesCommandHandler(IResourceWatcherResolver resourceWatcherResolver)
    : IRequestHandler<CopyResourceFilesCommand>
{
    public async Task Handle(CopyResourceFilesCommand request, CancellationToken cancellationToken)
    {
        foreach (var project in request.Config.Projects)
        {
            CopyProjectFiles(project);
        }

        var watcher = resourceWatcherResolver.Resolve(request.Config.Name);
        await watcher.FireAsync(ResourceWatcherTrigger.FilesCopied);
    }
    
    private static void CopyProjectFiles(ResourceProjectConfig resourceProject)
    {
        var filesToCopy = Directory.GetFiles(resourceProject.BuildDirectory);
        foreach (var file in filesToCopy)
        {
            File.Copy(file, Path.Combine(resourceProject.TargetDirectory, Path.GetFileName(file)), overwrite: true);
        }
    }
}