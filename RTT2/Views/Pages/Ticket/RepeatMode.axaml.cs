using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using RandomTick.Models;
using RandomTick.RandomTick.models;
using RandomTick.RandomTick.services;
using RandomTick.ViewModels;

namespace RandomTick.Views.Pages;

public partial class RepeatMode : Panel, IUiEventManage
{
    private readonly TickSetService _service;
    public RepeatModeViewModel Model { get; }
    private TicketSet? _currentSet;
    private bool _isStop = true;
    
    public RepeatMode()
    {
        App.RttApp.Server
            .GetService("TickSetService", out TickSetService? v);
        _service = v ?? throw new Exception("Error To Load Data");
        var names = v.GetSetsNames;
        
        InitializeComponent();
        DataContext = Model = new RepeatModeViewModel(names);
        
        OnInit();
    }

    public void OnInit()
    {
        Next.Click += NextOnClick;
        SetSelect.SelectionChanged += SetSelectOnSelectionChanged;
    }
    
    private void SetSelectOnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var itm = SetSelect.SelectedItem;
        if(itm is not string str) return;
        var set = _service[str];
        if(set == null) return;
        _currentSet = set;
    }

    
    private async void NextOnClick(object? sender, RoutedEventArgs e)
    {
        if(_currentSet == null) 
        {
            DialogExtend.ShowErrorDialog("您需要选择一个班级来继续", "找不到班级");
            return;
        }
        if (_isStop)
        {
            _isStop = false;
            Next.Content = "停止";
            await Task.Run(() =>
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
                    
                    while (_service.CheatFile?.IsInCheat(txt) ?? false) //FOR CHEATERS
                        txt = _currentSet.RandomNext();
                    
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        Display.Text = txt;
                        Display.TextAlignment = TextAlignment.Center;
                    });
                }
            });
        }
        else
        {
            _isStop = true;
            Next.Content = "开始";
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