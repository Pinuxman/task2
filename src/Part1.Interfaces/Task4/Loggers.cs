namespace Interfaces.Task4;

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[console] {message}");
}

public class FileLogger(string path) : ILogger
{
    public void Log(string message) =>
        File.AppendAllText(path, $"{DateTime.Now:HH:mm:ss} {message}{Environment.NewLine}");
}

public static class Worker
{
    public static void DoWork(ILogger logger)
    {
        logger.Log("Работа начата");
        logger.Log("Обработано 3 записи");
        logger.Log("Работа завершена");
    }
}
