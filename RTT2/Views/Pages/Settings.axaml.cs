using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ClsOom.ClassOOM.il8n;
using RandomTick.Models;

namespace RandomTick.Views.Pages;

public partial class Settings : Panel, IUiEventManage
{
    public Settings()
    {
        InitializeComponent();
        
        OnInit();
    }

    public void OnInit()
    {
        VSDevelop.IsCheckedChanged += VSDevelopOnIsCheckedChanged;
        VSDevelop.IsChecked = App.RTTApp.Config.IsDebugMode;
        SaveLang.Click += SaveLangOnClick;
        CheatEntrance.DoubleTapped += CheatEntranceOnClick;

        UpdateLanguageSelector();
    }

    private int _cd;
    private async void CheatEntranceOnClick(object? sender, RoutedEventArgs e)
    {
        _cd++;
        if (_cd != 3) return;
        _cd = 0;
            
        if(!App.RTTApp.Config.CheatMode) Process.Start("https://www.bilibili.com/video/BV1he4y1w7wB");
        var r = await DialogExtend.ShowInputDialog("CheatMode", "Write Down The Cheaters Below");
        var parts = r.Split("\n");
        App.RTTApp.Server.GetDataSubDir("チート", out var c);
        File.WriteAllLines($"{c}//悪質なチーター", parts);
    }

    private void SaveLangOnClick(object? sender, RoutedEventArgs e)
    {
        var type = (Il8NType?)VSLanguages.SelectedItem ?? Il8NType.ZhCn;
        App.RTTApp.Server.SetSelectedType(type);
        App.RTTApp.Config.SelectedLanguage.Set(type);
        DialogExtend.ShowErrorDialog("Although You Already Changed The Language Settings, But It May Take Whiles To Apply, We Suggest That You Can Restart This App", "✅Succeed!");
    }

    private void UpdateLanguageSelector()
    {
        App.RTTApp.Server.GetExistTypes(out var types)
            .GetSelectedType(out var s);
        VSLanguages.ItemsSource = types;
        VSLanguages.SelectedIndex = types.IndexOf(s);
    }

    private void VSDevelopOnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var ck = VSDevelop.IsChecked ?? false;
        App.RTTApp.Config.IsDebugMode.Set(ck);
    }

    public void OnKill()
    {
        
    }

    ~Settings()
    {
        OnKill();
    }
}