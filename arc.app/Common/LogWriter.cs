using Microsoft.Extensions.Logging;
using System;

namespace arc.app.Common
{
    public class LogWriter : ILogWriter
    {
        private readonly string _uniqueId;
        private ILogger _logger;

        public LogWriter(ILogger logger)
        {
            _uniqueId = Guid.NewGuid().ToString();
            _logger = logger;
        }

        public void AddLogger<T>(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message, string className, string method)
        {
            try
            {
                _logger.LogInformation("{unqiueId} : {className} : {method} : {message}", _uniqueId, className, method, message);
            }
            catch
            {
                // Logging must not affect request handling.
            }
        }

        public void LogWarning(string message, string className, string method)
        {
            try
            {
                _logger.LogWarning("{unqiueId} : {className} : {method} : {message}", _uniqueId, className, method, message);
            }
            catch
            {
                // Logging must not affect request handling.
            }
        }

        public void LogError(string message, string className, string method)
        {
            try
            {
                _logger.LogError("{unqiueId} : {className} : {method} : {message}", _uniqueId, className, method, message);
            }
            catch
            {
                // Logging must not affect request handling.
            }
        }
    }
}
