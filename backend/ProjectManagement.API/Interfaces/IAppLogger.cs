using System;

namespace PresentationLayer.Interfaces
{
    public interface IAppLogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception? ex = null);
    }
}