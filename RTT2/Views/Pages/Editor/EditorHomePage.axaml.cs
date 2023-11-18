using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ClsOom.ClassOOM.il8n;
using RandomTick.Models;
using RandomTick.ViewModels.Editor;

namespace RandomTick.Views.Pages.Editor;

public partial class EditorHomePage : Panel, IUiEventManage
{
    private readonly TagEditor _editor;
    private readonly EditorHomePageViewModel _context;

    [Il8N] public string? OpenText;
    [Il8N] public string? RefreshText;
    [Il8N] public string? DeleteText;
    [Il8N] public string? NewText;
    
    
    [Il8N("RandomTick.Dialogs.DeleteMsg")] public string? DeleteMsg;
    [Il8N("RandomTick.Dialogs.DeleteYes")] public string? DeleteYes;
    [Il8N("RandomTick.Dialogs.DeleteNo")] public string? DeleteNo;
    [Il8N("RandomTick.Dialogs.DeleteTitle")] public string? DeleteTitle;

    [Il8N] public string? NewSet;
    [Il8N] public string? NewSetTips;
    
    public EditorHomePage(TagEditor editor)
    {
        App.RTTApp.Server.UpdateFields(this);
        
        _editor = editor;
        DataContext = _context = new EditorHomePageViewModel();
        
        InitializeComponent();
        OnInit();
    }

    public void OnInit()
    {
        Files.DoubleTapped += FilesOnDoubleTapped;
        
        OpenBtn.Click += OpenBtnOnClick;
        OpenBtn.Label = OpenText;
        
        Files.ItemsSource = _context.Files;
        RefreshBtn.Click += RefreshBtnOnClick;
        RefreshBtn.Label = RefreshText;
        
        DeleteBtn.Click += DeleteBtnOnClick;
        DeleteBtn.Label = DeleteText;
        
        NewBtn.Click += NewBtnOnClick;
        NewBtn.Label = NewText;
    }

    private async void NewBtnOnClick(object? sender, RoutedEventArgs e)
    {
        var name = await DialogExtend.ShowInputDialog(NewSet!, NewSetTips!);
        if(name.Length <= 0) return;
        var s = _context.GetService();
        _ = s.NewSet(name);
        RefreshList();
        _editor.OpenDoc(name);
    }

    private void RefreshList()
    {
        _context.RefreshFiles();
        Files.ItemsSource = null;
        Files.Items.Clear();
        Files.ItemsSource = _context.Files;
    }
    
    private async void DeleteBtnOnClick(object? sender, RoutedEventArgs e)
    {
        if(Files.SelectedIndex == -1 || Files.SelectedItem == null)
            return;
        var f = (string)Files.SelectedItem;

        var r = await DialogExtend.ShowSelectDialog($"{DeleteMsg} {f}?", $"{DeleteYes}", $"{DeleteNo}", $"{DeleteTitle}");
        if(!r) return;
        
        var s = _context.GetService();
        s[f]?.DeleteSelf();
        RefreshList();
    }

    private void RefreshBtnOnClick(object? sender, RoutedEventArgs e)
        => RefreshList();

    private void OpenBtnOnClick(object? sender, RoutedEventArgs e)
    {
        if(Files.SelectedIndex == -1 || Files.SelectedItem == null)
            return;
        var f = (string)Files.SelectedItem;
        _editor.OpenDoc(f);
    }

    private void FilesOnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if(Files.SelectedIndex == -1 || Files.SelectedItem == null)
            return;
        var f = (string)Files.SelectedItem;
        _editor.OpenDoc(f);
    }

    public void OnKill()
    {
        Files.DoubleTapped -= FilesOnDoubleTapped;
        RefreshBtn.Click -= RefreshBtnOnClick;
        DeleteBtn.Click -= DeleteBtnOnClick;
        NewBtn.Click -= NewBtnOnClick;
        
    }

    ~EditorHomePage()
    {
        OnKill();
    }
}