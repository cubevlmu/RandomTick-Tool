using System.Collections.Concurrent;

namespace ClsOom.ClassOOM.loggers.services;

public class LogRecordService : ILoggerService
{
    private readonly StreamWriter _writer;
    private readonly BlockingCollection<string> _tasks = new();
    private bool _isEnd;

    public LogRecordService(ClassOomServer server, string name)
    {
        server.GetDataSubDir("logs", out var dir);
        var s = $"{dir}//{name}.log";
        _writer = File.AppendText(s);
        
        _tasks.Add("\nNew Record Started!");
        _tasks.Add($"Start At {DateTime.Now.Ticks}\n");
        
        _ = Task.Run(Runner);
    }

    private async Task Runner()
    {
        var v = _tasks.Take();
        await _writer.WriteLineAsync(v);
        await _writer.FlushAsync();

        if(_isEnd) return;
        _ = Task.Run(Runner);
    }
    
    public void OnLog(LogType type, string msg, string formatted)
    {
        _tasks.Add(formatted);
    }

    
    ~LogRecordService()
    {
        _isEnd = true;
        
        _writer.Flush();
        _writer.Close();
    }
}