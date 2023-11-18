namespace ClsOom.ClassOOM.loggers.services;

public class LogOutputService : ILoggerService
{
    public LogOutputService()
    {
        _backend.Start();
    }
    
    public void OnLog(LogType type, string msg, string formatted)
    {
        var index = formatted.IndexOf(type.GetLogTypeShortString(), StringComparison.Ordinal);
        var fPart = formatted[(index + 3)..];
        var sPart = formatted[..index];
        
        _backend.AddLogTask(fPart, sPart, type);
    }

    private readonly old.LoggerBackend _backend = old.LoggerBackend.FetchBackEnd();
}