using System.Text.Json;
using ResourceReload.Core.Features.Configuration;

namespace ResourceReload.AltV.Features.Configuration;

internal sealed class ConfigurationReader
{
    private readonly AltVLogger logger = new();
    private const string ConfigFilePath = "resources/ResourceReload/reloadconfig.json";
    
    internal ResourceReloadConfig? GetConfiguration()
    {
        var rawConfig = GetRawConfig();
        return ParseConfig(rawConfig);
    }

    private ResourceReloadConfig? ParseConfig(string? rawConfig)
    {
        if (string.IsNullOrWhiteSpace(rawConfig))
        {
            logger.LogError("Failed to parse resource reload configuration");
            return null;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        var config = JsonSerializer.Deserialize<ResourceReloadConfig>(rawConfig, jsonOptions);

        if (config is null)
        {
            logger.LogError("Failed to parse resource reload configuration");
        }

        return config;
    }

    private string? GetRawConfig()
    {
        if (!File.Exists(ConfigFilePath))
        {
            logger.LogError("Resource reload configuration not found");
            return null;
        }

        return File.ReadAllText(ConfigFilePath);
    }
}