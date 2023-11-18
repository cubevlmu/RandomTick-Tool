using System.Collections.Concurrent;

namespace ClsOom.ClassOOM.hosting;

[Obsolete("What The Fuck Is This")]
public partial class AppConsole
{
    public AppConsole()
    {
        StartPrintTask();
        GC.Collect();
    }

    public static void WriteColor(ConsoleColor color)
        => Console.ForegroundColor = color;
    public static void ResetColor()
        => Console.ResetColor();
    
    public void StartPrintTask()
    {
        _ = Task.Run(PrintTask);
        new Task(ConsoleReaderTask).Start();
        GC.Collect();
    }

    private async Task PrintTask()
    {
        var log = _logs.Take();
        WriteColor(log.Color);
        await Console.Out.WriteLineAsync(log.Log);
        await Console.Out.FlushAsync();
        ResetColor();
        _ = Task.Run(PrintTask);
    }

    public void AddPrintTask(LogTask task)
        => _logs.Add(task);

    private readonly BlockingCollection<LogTask> _logs = new();

    private readonly TextReader _reader = Console.In;
    private readonly TextWriter _writer = Console.Out;

    public static AppConsole ConsoleInstance = new AppConsole();
}

public struct LogTask
{
    public LogTask(string log) : this()
    {
        Color = ConsoleColor.White;
        Log = log;
    }
        
    internal LogTask(ConsoleColor color, string log) : this()
    {
        Color = color;
        Log = log;
    }

    public readonly ConsoleColor Color;
    public readonly string Log; 
}