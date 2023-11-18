using System;
using System.Collections.Generic;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using ReactiveUI;

namespace RandomTick.ViewModels;

public struct NavItem
{
    public NavItem(IResourceHost r, string t, Type page, string i, string? iS = null)
    {
        Text = t;
        PageType = page;
        
        //var icon = GetIcon(r, i);
        Icon = i;
        IconSelected = iS;

        // if (iS == null) return;
        // var iconSelected = GetIcon(r, iS);
        // if (iconSelected != null)
        //      = iconSelected;
    }

    private static IconSource? GetIcon(IResourceHost r, string key)
    {
        var rs = r.TryFindResource(key, out var v);
        if (!rs || v == null) return null;
        return (IconSource?)v;
    }
    
    private static IconSource FindAndSetDismiss(IResourceHost rh)
    {
        var r = GetIcon(rh, "Dismiss");
        if (r == null) throw new Exception("Fluent Icon Set Died");
        return r;
    }
    
    public string Text { get; }
    public string Icon { get; private set; }
    public string? IconSelected { get; }
    public Type PageType { get; }
}


public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(List<NavItem> navMenu, List<NavItem> fNavFooter)
    {
        NavMenu = navMenu;
        NavFooter = fNavFooter;
    }

    public MainWindowViewModel()
    {
        NavMenu = new List<NavItem>();
        NavFooter = new List<NavItem>();
    }

    
    public List<NavItem> NavMenu { get; }
    public List<NavItem> NavFooter { get; }
    
    
    private string _header = string.Empty;
    public string PageHeader
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value);
    }

    private object? _content;
    public object? PageContent
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    public NavItem? DefaultItem => null;

    internal void SetHeader(string header)
    {
        PageHeader = header; 
    }

    public void SwitchPage(Type pageType, string? title = null)
    {
        var result = Activator.CreateInstance(pageType);
        if (result is not UserControl control) return;
        
        var titleTag = title ?? ((string?)control.Tag ?? "[No Title]");
        
        PageHeader = titleTag;
        PageContent = control;
    }
    
    public void SwitchPageUseFrame(Type pageType, Frame f, string title)
    {
        var r = f.Navigate(pageType);
        if(!r) return;
        PageHeader = title;
    }
}