using System.Reflection;
using System.Runtime.Serialization;
using ResourceReload.Core.Common.Interfaces;

namespace ResourceReload.AltV.Features.AutoReconnect;

using Timer = System.Timers.Timer;

internal sealed class ReconnectService
{
    private const uint DebugPort = 9223;
    private const uint RetryDelay = 2500;

    private readonly ILogger logger;
    
    private readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri($"http://127.0.0.1:{DebugPort}")
    };
    
    private readonly Timer timer = new(RetryDelay);

    public ReconnectService(ILogger logger)
    {
        this.logger = logger;
    }

    internal async Task ConnectLocalClient()
    {
        var status = await GetLocalClientStatus();
        logger.LogInformation($"Client status: {status}");
        
        switch (status)
        {
            case ClientStatus.Error:
                break;
            case not ClientStatus.MainMenu and ClientStatus.InGame when !timer.Enabled:
                timer.Start();
                break;
            case ClientStatus.InGame when timer.Enabled:
                timer.Stop();
                break;
            default:
                try
                {
                    await httpClient.PostAsync("/reconnect", null);
                }
                catch (Exception e)
                {
                    logger.LogError($"Reconnect failed: {e.Message}");
                }

                break;
        }
    }

    private async Task<ClientStatus> GetLocalClientStatus()
    {
        try
        {
            var response = await httpClient.GetStringAsync("/status");
            
            var enumValue = typeof(ClientStatus).GetFields()
                .FirstOrDefault(f => f.GetCustomAttribute<EnumMemberAttribute>()?.Value == response)?
                .GetValue(null);
            
            return enumValue != null ? (ClientStatus)enumValue : ClientStatus.Error;
        }
        catch (Exception e)
        {
            logger.LogError($"Failed to fetch local client status {e.Message}");
            return ClientStatus.Error;
        }
    }
}