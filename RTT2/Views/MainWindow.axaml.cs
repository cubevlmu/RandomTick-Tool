using System;
using System.Collections.Generic;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using RandomTick.Models;
using RandomTick.RandomTick.seat;
using RandomTick.RandomTick.services;
using RandomTick.RandomTick.ticket;
using RandomTick.ViewModels;
using RandomTick.Views.Pages;

namespace RandomTick.Views;

public partial class MainWindow : AppWindow, IUiEventManage
{

    private readonly MainWindowViewModel _model;
    private readonly TickSetService _service;
    
    public MainWindow()
    {
        
        TitleBar.ExtendsContentIntoTitleBar = true;
        
        DataContext ??= new MainWindowViewModel(new List<NavItem>
        {
            new(this, "抽签工具", () => new TagTool(), "\ue82d"),
            new(this, "座位工具", () => new SeatPage(), "\uf0e2"),
            new(this, "班级编辑", () => new TagEditor(), "\ue70f")
        }, new List<NavItem>
        {
            new(this, "设置", () => new Settings(), "\ue713")
        });
        _model = (MainWindowViewModel)DataContext;

        InitializeComponent();
        Title = App.RttApp.Config.IsDebugMode ? $"{Title} [调试模式]" : Title;
        
        OnInit();
        
        App.RttApp.Server.GetService("TickSetService", out TickSetService? v);
        _service = v ?? throw new Exception("Error To Load Data");
    }

    public async void OnInit()
    {
        App.RttApp.Server.GetLogger("CheatEncoder", out var logger);
        
        var rr = Program.Args.ParseArguments();
        var cc = rr["compile"];
        var state = rr["state"];
        if (cc != "" || state != "")
        {
            logger.Info("Begin encode task");
            switch (state)
            {
                case "ticket":
                {
                    logger.Info("State == ticket");
                    var ce = new TicketCheatEncoder(cc);
                    await ce.Begin();
                    await ce.End();
                    break;
                }
                case "seat":
                {
                    logger.Info("State == seat");
                    var ce = new CheatEncoder(cc);
                    await ce.Begin();
                    await ce.End();
                    break;
                }
            }

            logger.Info("Cheat file encoder task done!");
            logger.Info("System will load it automatically");
            Environment.Exit(0);
        }
        
        // Pre Init the seat builder : read the cheat file before seat builder use it
        try
        {
            await SeatBuilder.PreInit();
            await _service.Setup();
        }
        catch (Exception e)
        {
            //logger.Error(e);
        }
        // ====

        NavigationView.SelectionChanged += NavigationViewOnSelectionChanged;

        NavigationView.SelectedItem = _model.NavMenu[0];
    }

    
    private void NavigationViewOnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        try
        {
            if (NavigationView.SelectedItem is not NavItem item) return;
            _model.SwitchPageUseFrame(item.PageTypeCreate.Invoke(), Frame, item);
        }
        catch (Exception ex)
        {
            App.RttApp.Server.GetLogger("RTT", out var logger);
            logger.Error(ex);
        }
    }


    public void OnKill()
    {
        NavigationView.SelectionChanged -= NavigationViewOnSelectionChanged;
    }

    ~MainWindow() => OnKill();
}