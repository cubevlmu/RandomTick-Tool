using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ClsOom.ClassOOM.il8n;
using RandomTick.Models;
using RandomTick.RandomTick.models;
using RandomTick.RandomTick.services;
using RandomTick.ViewModels;

namespace RandomTick.Views.Pages;

public partial class RepeatMode : Panel, IUiEventManage
{
    private readonly TickSetService _service;
    private readonly RepeatModeViewModel _model;
    private TicketSet? _currentSet;
    private bool _isStop = true;

    [Il8N("RandomTick.Views.Pages.HomePage.DisplayDefault")] 
    public string? DisplayDefault;
    [Il8N("RandomTick.Views.Pages.HomePage.NextDefault")] 
    public string? NextDefault;
    [Il8N("RandomTick.Views.Pages.HomePage.SelectedDefault")] 
    public string? SelectedDefault;
    [Il8N] public string? SpeedText;
    [Il8N] public string? NextEnd;
    [Il8N] public string? NextStart;
    
    [Il8N("RandomTick.Dialogs.EmptySelectMsg")] 
    public string? EmptySelectMsg;
    [Il8N("RandomTick.Dialogs.DialogErrorTitle")] 
    public string? DialogErrorTitle;

    private string[] _cs = Array.Empty<string>();

    public RepeatMode()
    {
        App.RTTApp.Server
            .UpdateFields(this)
            .GetService("TickSetService", out TickSetService? v);
        _service = v ?? throw new Exception("Error To Load Data");
        var names = v.GetSetsNames;
        
        InitializeComponent();
        DataContext = _model = new RepeatModeViewModel(names);
        
        OnInit();
    }
    
    
    private async void Filter()
    {
        if(!App.RTTApp.Config.CheatMode) return;
        App.RTTApp.Server.GetDataSubDir("チート", out var c);
        if(!File.Exists($"{c}//悪質なチーター")) return;
        _cs = await File.ReadAllLinesAsync($"{c}//悪質なチーター");
    }

    public void OnInit()
    {
        Next.Click += NextOnClick;
        SetSelect.SelectionChanged += SetSelectOnSelectionChanged;

        Display.Text = DisplayDefault;
        NextDisplay.Text = NextDefault;
        Next.Content = NextStart;
        SetDisplay.Text = SelectedDefault;
        SpeedDisplay.Text = SpeedText;

        Filter();
    }
    
    private void SetSelectOnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var itm = SetSelect.SelectedItem;
        if(itm is not string str) return;
        var set = _service[str];
        if(set == null) return;
        _currentSet = set;
    }

    
    private void NextOnClick(object? sender, RoutedEventArgs e)
    {
        if(_currentSet == null) 
        {
            DialogExtend.ShowErrorDialog(EmptySelectMsg!, DialogErrorTitle!);
            return;
        }
        if (_isStop)
        {
            _isStop = false;
            Next.Content = NextEnd;
            new Task(() =>
            {
                if(_currentSet == null) return;
                var sp = 1;
                Dispatcher.UIThread.Invoke(() =>
                {
                    sp = (int)Math.Round(SpeedSelect.Value);
                });

                while (!_isStop)
                {
                    Thread.Sleep(100 * (10-sp));
                    var txt = _currentSet.RandomNext();
                    
                    while (_cs.Contains(txt)) //FOR CHEATERS
                        txt = _currentSet.RandomNext();
                    
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        Display.Text = txt;
                    });
                }
            }).Start();
        }
        else
        {
            _isStop = true;
            Next.Content = NextStart;
        }
    }

    
    public void OnKill()
    {
        Next.Click -= NextOnClick;
        SetSelect.SelectionChanged -= SetSelectOnSelectionChanged;
    }

    ~RepeatMode()
    {
        OnKill();
    }
}