using ApplicationLayer.Services;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Singleton
{
    // Wrapper para o LoggerService. Permite que seja injetada como uma dependência em outras partes da aplicação
    public class AppLogger : IAppLogger
    {
        public void LogInfo(string message)
        {
            LoggerService.Instance.Log(message);
        }
    }
}