using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using RandomTick.Models;
using RandomTick.RandomTick.services;
using RandomTick.ViewModels;

namespace RandomTick.Views.Pages;

public partial class NormalMode : Panel, IUiEventManage
{
    private readonly TickSetService _service;
    
    public NormalMode()
    {
        App.RttApp.Server.GetService("TickSetService", out TickSetService? v);
        _service = v ?? throw new Exception("Error To Load Data");
        var names = v.GetSetsNames;
        
        var model = new HomePageViewModel(names);
        DataContext = model;

        InitializeComponent();
        OnInit();
    }


    public void OnInit()
    {
        Next.Click += NextOnClick;
        //Display.AddFadeAnimation();
    }
    
    
    private void NextOnClick(object? sender, RoutedEventArgs e)
    {
        var itm = SetSelect.SelectedItem;
        if (itm is not string str)
        {
            DialogExtend.ShowErrorDialog("您需要选择一个班级来继续", "找不到班级");
            return;
        }
        var set = _service[str];
        if(set == null)
        {
            DialogExtend.ShowErrorDialog("选择的班级是无效的，请检查文件或者相关日志!");
            return;
        }
        var v = set.RandomNext();
        
        while (_service.CheatFile?.IsInCheat(v) ?? false) //FOR CHEATERS
            v = set.RandomNext();
        
        Display.Text = v;
        Display.TextAlignment = TextAlignment.Center;
    }

    
    public void OnKill()
    {
        Next.Click -= NextOnClick;
    }

    ~NormalMode()
    {
        OnKill();
    }
}