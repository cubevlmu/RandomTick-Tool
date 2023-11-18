using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClsOom.ClassOOM.il8n;
using RandomTick.Models;
using RandomTick.RandomTick.services;
using RandomTick.ViewModels;

namespace RandomTick.Views.Pages;

public partial class HomePage : Panel, IUiEventManage
{
    public HomePage()
    {
        App.RTTApp.Server.GetService("TickSetService", out TickSetService? v);
        _service = v ?? throw new Exception("Error To Load Data");
        var names = v.GetSetsNames;
        
        _model = new HomePageViewModel(names);
        DataContext = _model;

        App.RTTApp.Server.UpdateFields(this);
        
        InitializeComponent();
        OnInit();
    }


    public void OnInit()
    {
        Next.Click += NextOnClick;
        //Display.AddFadeAnimation();

        Display.Text = DisplayDefault;
        Next.Content = NextDefault;
        NextDisplay.Text = NextDefault;
        TheSetDisplay.Text = SelectedDefault;
        
        Filter();
    }
    
    
    private async void Filter()
    {
        if(!App.RTTApp.Config.CheatMode) return;
        App.RTTApp.Server.GetDataSubDir("チート", out var c);
        if(!File.Exists($"{c}//悪質なチーター")) return;
        _cs = await File.ReadAllLinesAsync($"{c}//悪質なチーター");
    }

    
    private void NextOnClick(object? sender, RoutedEventArgs e)
    {
        var itm = SetSelect.SelectedItem;
        if (itm is not string str)
        {
            DialogExtend.ShowErrorDialog(EmptySelectMsg!, DialogErrorTitle!);
            return;
        }
        var set = _service[str];
        if(set == null)
        {
            DialogExtend.ShowErrorDialog("Selected Set Is Error!");
            return;
        }
        var v = set.RandomNext();
        
        while (_cs.Contains(v)) //FOR CHEATERS
            v = set.RandomNext();
        
        Display.Text = v;
    }

    
    public void OnKill()
    {
        Next.Click -= NextOnClick;
    }

    ~HomePage()
    {
        OnKill();
    }

    [Il8N] public string? DisplayDefault;
    [Il8N] public string? NextDefault;
    [Il8N] public string? SelectedDefault;
    
    [Il8N("RandomTick.Dialogs.EmptySelectMsg")] 
    public string? EmptySelectMsg;
    [Il8N("RandomTick.Dialogs.DialogErrorTitle")] 
    public string? DialogErrorTitle;
    
    
    private string[] _cs = Array.Empty<string>();
    private readonly HomePageViewModel _model;
    private readonly TickSetService _service;
}