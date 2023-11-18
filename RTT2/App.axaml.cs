using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RandomTick.RandomTick;
using RandomTick.Views;

namespace RandomTick;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.Closed += (_, _) =>
            {
                RTTApp.Server.EndSession();
            };
        }
        
        Console.WriteLine(RTTApp.Config.AppTitle);
        
        RTTApp.Server.GetServiceFunc<int, int>(
            "TestService", 
            "TestFunction", 
            out var s
        );
        s?.Invoke(0);

        RTTApp.Server.GetServiceField<int>(
            "TestService",
            "TestField",
            out var f
        );
        Console.WriteLine($"Field Value Is {f?.Get()}");
        

        base.OnFrameworkInitializationCompleted();
    }

    public static readonly RandomTickApp RTTApp = new();
}