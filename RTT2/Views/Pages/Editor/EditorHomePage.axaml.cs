using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using RandomTick.Models;
using RandomTick.ViewModels.Editor;

namespace RandomTick.Views.Pages.Editor;

public partial class EditorHomePage : Panel, IUiEventManage
{
    private readonly TagEditor _editor;
    private readonly EditorHomePageViewModel _context;

    public EditorHomePage(TagEditor editor)
    {
        _editor = editor;
        DataContext = _context = new EditorHomePageViewModel();
        
        InitializeComponent();
        OnInit();
    }

    public void OnInit()
    {
        Files.DoubleTapped += FilesOnDoubleTapped;
        
        OpenBtn.Click += OpenBtnOnClick;
        
        Files.ItemsSource = _context.Files;
        RefreshBtn.Click += RefreshBtnOnClick;
        
        DeleteBtn.Click += DeleteBtnOnClick;
        
        NewBtn.Click += NewBtnOnClick;

        if (Files.Items.Count > 0)
            Files.SelectedIndex = 0;
    }

    private async void NewBtnOnClick(object? sender, RoutedEventArgs e)
    {
        var name = await DialogExtend.ShowInputDialog("🆕新建签集", "给你的签集起一个好名字吧~ :)");
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
        if (Files.SelectedIndex == -1 || Files.SelectedItem == null)
        {
            DialogExtend.ShowErrorDialog("请选择一个班级来继续！");
            return;
        }

        var f = (string)Files.SelectedItem;

        var r = await DialogExtend.ShowSelectDialog($"是否要删除班级 {f}?", "是的", "不要", "删除班级确认");
        if(!r) return;
        
        var s = _context.GetService();
        s[f]?.DeleteSelf();
        RefreshList();
    }

    private void RefreshBtnOnClick(object? sender, RoutedEventArgs e)
        => RefreshList();

    private void OpenBtnOnClick(object? sender, RoutedEventArgs e)
    {
        if (Files.SelectedIndex == -1 || Files.SelectedItem == null)
        {
            DialogExtend.ShowErrorDialog("请选择一个班级来继续！");
            return;
        }
        var f = (string)Files.SelectedItem;
        _editor.OpenDoc(f);
    }

    private void FilesOnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (Files.SelectedIndex == -1 || Files.SelectedItem == null)
        {
            DialogExtend.ShowErrorDialog("请选择一个班级来继续！");
            return;
        }
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