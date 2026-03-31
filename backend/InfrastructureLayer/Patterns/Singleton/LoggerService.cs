using System;
using System.IO;
using System.Text;

namespace InfrastructureLayer.Patterns.Singleton
{
    public sealed class LoggerService
    {
        // O Lazy assegura a complexidade da criação segura
        private static readonly Lazy<LoggerService> _lazyInstance = new Lazy<LoggerService>(() => new LoggerService());

        // Caminho da diretoria de logs
        private readonly string _logsDirectory;

        // Lock exclusivo para a escrita no ficheiro
        private readonly object _fileLock = new object();

        // Enum para o tipo de log
        private enum LogLevel { INFO, WARNING, ERROR }

        private LoggerService()
        {
            // Criação da pasta "Logs"
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _logsDirectory = Path.Combine(baseDir, "Logs");

            // Se a pasta "Logs" não existir, cria-a automaticamente
            if (!Directory.Exists(_logsDirectory))
            {
                Directory.CreateDirectory(_logsDirectory);
            }

            LogInfo("--- LoggerService inicializado ---");
        }

        // Propriedade para aceder à instância
        public static LoggerService Instance => _lazyInstance.Value;

        // Log file diário com base na data
        private string GetCurrentLogFilePath()
        {
            string dateSuffix = DateTime.Now.ToString("yyyy-MM-dd");
            return Path.Combine(_logsDirectory, $"system_logs_{dateSuffix}.log");
        }

        // Método para escrever o log
        private void WriteLog(LogLevel level, string message, Exception? ex = null)
        {
            var sb = new StringBuilder();
            sb.Append($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] [{level}] {message}");

            // Adicionar o Stack Trace se for um erro e trouxer a exceção
            if (ex != null)
            {
                sb.AppendLine();
                sb.Append($"   EXCEPTION: {ex.Message}");
                sb.AppendLine();
                sb.Append($"   STACK TRACE: {ex.StackTrace}");
            }

            string logEntry = sb.ToString() + Environment.NewLine;

            // Mudar a cor da consola dependendo do nível do log
            ConsoleColor originalColor = Console.ForegroundColor;

            Console.ForegroundColor = level switch
            {
                LogLevel.INFO => ConsoleColor.Cyan,      // Ciano para Info
                LogLevel.WARNING => ConsoleColor.Yellow, // Amarelo para Avisos
                LogLevel.ERROR => ConsoleColor.Red,      // Vermelho para Erros
                _ => originalColor
            };

            // Imprime na consola
            Console.Write(logEntry);

            // Restaura a cor original
            Console.ForegroundColor = originalColor;

            // Escreve no ficheiro do dia em segurança
            lock (_fileLock)
            {
                File.AppendAllText(GetCurrentLogFilePath(), logEntry);
            }
        }

        public void LogInfo(string message) => WriteLog(LogLevel.INFO, message);

        public void LogWarning(string message) => WriteLog(LogLevel.WARNING, message);

        public void LogError(string message, Exception? ex = null) => WriteLog(LogLevel.ERROR, message, ex);
    }
}