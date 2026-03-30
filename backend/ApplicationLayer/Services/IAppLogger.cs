namespace ApplicationLayer.Services
{
    // Interface para o Logger da AplicationLayer
    public interface IAppLogger
    {
        void LogInfo(string message);
        void Error (string message, Exception ex);
        void Warning (string message);
    }
}