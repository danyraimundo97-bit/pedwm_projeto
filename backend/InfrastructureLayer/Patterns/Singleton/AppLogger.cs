using System;
using ApplicationLayer.Services;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Singleton
{
    // Wrapper para o LoggerService. 
    public class AppLogger : IAppLogger
    {
        public void LogInfo(string message)
        {
            LoggerService.Instance.LogInfo(message);
        }

        public void LogWarning(string message)
        {
            LoggerService.Instance.LogWarning(message);
        }

        public void LogError(string message, Exception? ex = null)
        {
            LoggerService.Instance.LogError(message, ex);
        }
    }
}