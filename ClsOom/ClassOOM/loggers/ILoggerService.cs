namespace ClsOom.ClassOOM.loggers;

public enum LogType
{
    Info, Warn, Error, Debug
}

public static class LogTypeExtend
{
    public static string GetLogTypeShortString(this LogType type)
    {
        return type switch
        {
            LogType.Info => "[I]",
            LogType.Warn => "[W]",
            LogType.Error => "[E]",
            LogType.Debug => "[D]",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static ConsoleColor GetLogTypeConsoleColor(this LogType type)
    {
        return type switch
        {
            LogType.Info => ConsoleColor.Cyan,
            LogType.Warn => ConsoleColor.Yellow,
            LogType.Error => ConsoleColor.Red,
            LogType.Debug => ConsoleColor.Green,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}

public interface ILoggerService
{
    void OnLog(LogType type, string msg, string formatted);
}