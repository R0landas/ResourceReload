using MediatR;
using ResourceReload.Core.Common.Interfaces;
using ResourceReload.Core.Features.Configuration;
using ResourceReload.Core.ResourceWatcherStateMachine;
using ResourceReload.Core.Util;

namespace ResourceReload.Core.Features.CopyResourceFiles;

internal sealed class CopyResourceFilesCommandHandler(IResourceWatcherResolver resourceWatcherResolver, ILogger logger)
    : IRequestHandler<CopyResourceFilesCommand>
{
    public async Task Handle(CopyResourceFilesCommand request, CancellationToken cancellationToken)
    {
        
        foreach (var project in request.Config.Projects)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await CopyProjectFiles(project, cts.Token);
        }

        var watcher = resourceWatcherResolver.Resolve(request.Config.Name);
        await watcher.FireAsync(ResourceWatcherTrigger.FilesCopied);
    }
    
    private async Task CopyProjectFiles(ResourceProjectConfig resourceProject, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        
        var filesToCopy = Directory.GetFiles(resourceProject.BuildDirectory);
        foreach (var file in filesToCopy)
        {
            await CopyFile(file, Path.Combine(resourceProject.TargetDirectory, Path.GetFileName(file)), token);
        }
    }

    private async Task CopyFile(string file, string destPath, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        try
        {
            File.Copy(file, destPath, overwrite: true);
        }
        catch (IOException e)
        {
            logger.LogError($"Failed to copy files {file} {e.Message}");
            logger.LogInformation("Trying again in 2 seconds...");
            await Task.Delay(2, token);
        }
    }
}