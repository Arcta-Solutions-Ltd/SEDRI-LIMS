using Microsoft.Extensions.Logging;

namespace arc.app.Common
{
    public interface ILogWriter
    {
        void LogInfo(string message, string className, string method);
        void LogWarning(string message, string className, string method);
        void LogError(string message, string className, string method);
        void AddLogger<T>(ILogger<T> logger);
    }
}
