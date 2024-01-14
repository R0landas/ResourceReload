using MediatR;
using ResourceReload.Core.Features.Configuration;

namespace ResourceReload.Core.Features.CopyResourceFiles;

public record CopyResourceFilesCommand(ResourceConfig Config) : IRequest;