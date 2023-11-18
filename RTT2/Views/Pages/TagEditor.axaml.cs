using System;
using System.Collections;
using Avalonia.Controls;
using ClsOom.ClassOOM.il8n;
using FluentAvalonia.UI.Controls;
using RandomTick.Models;
using RandomTick.RandomTick.services;
using RandomTick.ViewModels;
using RandomTick.Views.Pages.Editor;

namespace RandomTick.Views.Pages;

public partial class TagEditor : UserControl, IUiEventManage
{
    private readonly TickSetService _service;
    private readonly TagEditorViewModel _context;

    [Il8N] public string? HomeTag;
    [Il8N("RandomTick.Dialogs.FileNotFound")] 
    public string? FileNotFoundMsg;
    [Il8N] public string? TitleTips;
    

    public TagEditor()
    {
        App.RTTApp.Server.UpdateFields(this);
        
        DataContext = _context = new TagEditorViewModel(this);
        App.RTTApp.Server.GetService("TickSetService", out TickSetService? v);
        _service = v ?? throw new Exception("Error To Load Data");
        
        InitializeComponent();
        OnInit();
    }

    public void OnInit()
    {
        //ViewTab.TabItems = _context.TabPages;
        ViewTab.AddTabButtonClick += ViewTabOnAddTabButtonClick;
        ViewTab.TabCloseRequested += ViewTabOnTabCloseRequested;
        
        
        var lst = (IList)ViewTab.TabItems;
        lst.Add(new TabViewItem
        {
            Header = HomeTag,
            IsClosable = false,
            IconSource = new SymbolIconSource
            {
                Symbol = Symbol.HomeFilled
            },
            Content = new EditorHomePage(this)
        });
    }

    private static void ViewTabOnTabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        var tab = args.Tab;
        var ct = tab.Content;
        if(ct is not EditorPage editor) return;
        editor.CallSave();
        var lst = (IList)sender.TabItems;
        lst.Remove(tab);
    }

    private static void ViewTabOnAddTabButtonClick(TabView sender, EventArgs args)
    { }

    public void OnKill()
    {
        ViewTab.AddTabButtonClick -= ViewTabOnAddTabButtonClick;
        ViewTab.TabCloseRequested -= ViewTabOnTabCloseRequested;
    }

    public void OpenDoc(string name)
    {
        var file = _service[name];
        if (file == null)
        {
            DialogExtend.ShowErrorDialog(FileNotFoundMsg!);
            return;
        }

        var lst = (IList)ViewTab.TabItems;
        lst.Add(new TabViewItem
        {
            Header = $"{TitleTips} {name}",
            IconSource = new SymbolIconSource
            {
                Symbol = Symbol.Document
            },
            Content = new EditorPage(file)
        });
        ViewTab.SelectedItem = lst[^1];
    }

    ~TagEditor()
    {
        OnKill();
    }
}