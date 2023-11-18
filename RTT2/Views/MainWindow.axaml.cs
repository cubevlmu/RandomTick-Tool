using System;
using System.Collections.Generic;
using Avalonia;
using ClsOom.ClassOOM.il8n;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using RandomTick.Models;
using RandomTick.ViewModels;
using RandomTick.Views.Pages;

namespace RandomTick.Views;

public partial class MainWindow : AppWindow, IUiEventManage
{
    public MainWindow()
    {
        App.RTTApp.Server.UpdateFields(this);
        TitleBar.ExtendsContentIntoTitleBar = true;
        
        DataContext ??= new MainWindowViewModel(new List<NavItem>
        {
            new(this, NormalModeText!, typeof(HomePage), "Home"),
            new(this, RepeatModeText!, typeof(RepeatMode), "Refresh"),
            new(this, EditorText!, typeof(TagEditor), "Contact")
        }, new List<NavItem>
        {
            new(this, SettingsText!, typeof(Settings), "Setting")
        });
        _model = (MainWindowViewModel)DataContext;
        
        InitializeComponent();
        base.Title = App.RTTApp.Config.IsDebugMode ? $"{this.Title} [DeveloperMode]" : this.Title;
        
        var rects = new[]
        {
            new Rect(0, 0, 100, 32),
            new Rect(300, 0, 100, 32)
        };

        TitleBar.SetDragRectangles(rects);
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        
        OnInit();
    }

    public void OnInit()
    {
        NavigationView.SelectionChanged += NavigationViewOnSelectionChanged;

        NavigationView.SelectedItem = _model.NavMenu[0];
    }

    
    private void NavigationViewOnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        try
        {
            if (NavigationView.SelectedItem is not NavItem item) return;
            _model.SwitchPageUseFrame(item.PageType, Frame, item.Text);
        }
        catch (Exception ex)
        {
            App.RTTApp.Server.GetLogger("RTT", out var logger);
            logger.Error(ex);
        }
    }


    public void OnKill()
    {
        NavigationView.SelectionChanged -= NavigationViewOnSelectionChanged;
    }

    ~MainWindow() => OnKill();

    [Il8N] public string? NormalModeText;
    [Il8N] public string? RepeatModeText;
    [Il8N] public string? EditorText;
    [Il8N] public string? SettingsText;
    [Il8N] public string? Title;

    private readonly MainWindowViewModel _model;
}