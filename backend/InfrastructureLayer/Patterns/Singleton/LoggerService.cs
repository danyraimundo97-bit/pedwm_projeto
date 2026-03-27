namespace InfrastructureLayer.Patterns.Singleton
{
    public sealed class LoggerService
    {
        // O Lazy assegura a complexidade da criação segura
        private static readonly Lazy<LoggerService> _lazyInstance = new Lazy<LoggerService>(() => new LoggerService());

        private readonly string _logFilePath;

        // Lock exclusivo para a escrita no ficheiro
        private readonly object _fileLock = new object();

        // Construtor Privado
        private LoggerService()
        {
            // Define que o ficheiro se vai chamar "system_logs.log" e ficará na pasta onde a API corre
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(baseDir, "system_logs.log");

            Log($"--- LoggerService inicializado ---");
        }

        // Propriedade para aceder à instância
        public static LoggerService Instance => _lazyInstance.Value;

        // Método de log
        public void Log(string message)
        {
            // Mensagem com a data e quebra de linha
            string logEntry = $"[LOG - {DateTime.Now:dd/MM/yyyy HH:mm:ss}] {message}{Environment.NewLine}";

            // Imprime na consola
            Console.Write(logEntry);

            // Escreve no ficheiro em segurança (com lock)
            lock (_fileLock)
            {
                File.AppendAllText(_logFilePath, logEntry);
            }
        }

        //TODO: Poderíamos adicionar métodos adicionais para diferentes níveis de log (ex: LogInfo, LogWarning, LogError) e incluir mais detalhes (ex: stack trace para erros)
        //TODO: Poderíamos implementar uma rotação de ficheiros para evitar que o ficheiro de log cresça indefinidamente (ex: criar um novo ficheiro a cada dia ou quando atingir um certo tamanho)
    }
}