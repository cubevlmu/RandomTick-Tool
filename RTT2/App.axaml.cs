using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RandomTick.RandomTick;
using RandomTick.RandomTick.seat;
using RandomTick.RandomTick.ticket;
using RandomTick.Views;

namespace RandomTick;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await RttApp.Init();
            
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.Closed += (_, _) =>
            {
                RttApp.Server.EndSession();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static readonly RandomTickApp RttApp = new();
}