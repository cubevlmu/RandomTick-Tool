using System.Text;
using ClsOom.ClassOOM.loggers.services;

namespace ClsOom.ClassOOM.loggers;

public class LoggerBackend : ILogger
{
    private readonly string _name;
    private readonly ILoggerService[] _services;

    public LoggerBackend(LogRecordService? service, string name)
    {
        _name = name;
        if (service == null)
        {
            _services = Array.Empty<ILoggerService>();
        }
        else
        {
            _services = new ILoggerService[]
            {
                new LogOutputService(),
                service
            };
        }
    }
    
    private void Log(LogType type, string msg)
    {
        _builder.Clear();
        _builder.Append(Format);
        _builder.Replace("[Time]", $"[{GetTime()}]")
            .Replace("[Type]", type.GetLogTypeShortString())
            .Replace("[ThreadID]", $"[{Environment.CurrentManagedThreadId}]")
            .Replace("[Sender]", $"{_name}")
            .Replace("[Log]", $"{msg}");
        var str = _builder.ToString();
        foreach (var loggerService in _services)
            loggerService.OnLog(type, msg, str);
    }

    public void Info(object msg) => Log(LogType.Info, msg.ToString() ?? "");
    public void Info(string template, params object[] msg) 
        => Info((object)string.Format(template, msg));
    public void Info(params object[] msg)
    {
        _builder.Clear();
        foreach (var o in msg) _builder.Append(o);
        Info(_builder.ToString());
        _builder.Clear();
    }

    
    public void Error(object msg) => Log(LogType.Error, msg.ToString() ?? "");
    public void Error(string template, params object[] msg)
        => Error((object)string.Format(template, msg));
    public void Error(params object[] msg)
    {
        _builder.Clear();
        foreach (var o in msg) _builder.Append(o);
        Error(_builder.ToString());
        _builder.Clear();
    }
    public void Error(Exception e)
    {
        Warn("|- Exception Type    : {0}", e.GetType().ToString());
        Warn("|- Exception Msg     : {0}", e.Message);
        Warn("|- Exception HResult : {0}", e.HResult.ToString());
        foreach(var line in e.StackTrace!.Split("\n"))
            Warn("|= {0}", line);
    }

    
    public void Warn(object msg) => Log(LogType.Warn, msg.ToString() ?? "");
    public void Warn(string template, params object[] msg)
        => Warn((object)string.Format(template, msg));
    public void Warn(params object[] msg)
    {
        _builder.Clear();
        foreach (var o in msg) _builder.Append(o);
        Warn(_builder.ToString());
        _builder.Clear();
    }
    public void Warn(Exception e)
    {
        Warn("|- Exception Type    : {0}", e.GetType().ToString());
        Warn("|- Exception Msg     : {0}", e.Message);
        Warn("|- Exception HResult : {0}", e.HResult.ToString());
        foreach(var line in e.StackTrace!.Split("\n"))
            Warn("|= {0}", line);
    }

    
    public void Debug(object msg)
    {
#if DEBUG
        Log(LogType.Debug, msg.ToString() ?? "");
#endif
    }
    public void Debug(string template, params object[] msg)
        => Debug((object)string.Format(template, msg));
    public void Debug(params object[] msg)
    {
        _builder.Clear();
        foreach (var o in msg) _builder.Append(o);
        Warn(_builder.ToString());
        _builder.Clear();
    }

    private static string GetTime() => DateTime.Now.ToLocalTime().ToString();
    private readonly StringBuilder _builder = new();
    private const string Format = "[Time] [Type] [Sender] : [Log]";
}