namespace ResourceReload.Core.Features.Configuration;

public class ResourceConfig
{
    public string Name { get; set; } = string.Empty;
    public IEnumerable<ResourceProjectConfig> Projects { get; set; } = [];
}