using ResourceReload.Core.Features.Configuration;

namespace ResourceReload.AltV.Features.Configuration;

public class AltVResourceReloadConfig : ResourceReloadConfig
{
    public bool UseAutoReconnect { get; set; }
}