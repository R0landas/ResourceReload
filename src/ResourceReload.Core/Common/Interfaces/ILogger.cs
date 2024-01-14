namespace ResourceReload.Core.Common.Interfaces;

public interface ILogger
{
    void LogInformation(string message);
    void LogError(string message);
}