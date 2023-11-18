﻿using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace RandomTick;

public static class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseAvaloniaNative()
            .UseReactiveUI();
}