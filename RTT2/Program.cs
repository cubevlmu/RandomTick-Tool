using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace RandomTick;

public static class Program
{
    public static bool HasArguments = false;
    public static string[]? Args;

    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            HasArguments = true;
            Args = args;
        }
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUI();
}