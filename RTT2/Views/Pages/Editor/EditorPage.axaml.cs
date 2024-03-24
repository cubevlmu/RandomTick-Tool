using Avalonia.Controls;
using Avalonia.Interactivity;
using ClsOom.ClassOOM.il8n;
using RandomTick.Models;
using RandomTick.RandomTick.models;
using RandomTick.ViewModels.Editor;

namespace RandomTick.Views.Pages.Editor;

public partial class EditorPage : Panel, IUiEventManage
{
    private readonly TicketSet _set;
    private readonly EditorPageViewModel _context;
    private bool _isEdited;

    public void OnInit()
    {
        TicketDisplay.ItemsSource = _context.ContentItems;
        RemoveBtn.Click += RemoveBtnOnClick;
        
        AddBtn.Click += AddBtnOnClick;
        
        SaveBtn.Click += SaveBtnOnClick;
        
        RenameBtn.Click += RenameBtnOnClick;
        
        DeleteSelfBtn.Click += DeleteSelfBtnOnClick;
    }
    
    public EditorPage(TicketSet set)
    {
        _set = set;

        DataContext = _context = new EditorPageViewModel(set);
        InitializeComponent();
        OnInit();
    }

    public void Save()
    {
        _isEdited = false;
        _set.ApplyAll();
        _set.Save();
        DialogExtend.ShowErrorDialog("保存班级成功！", "保存成功");
    }

    public async void CallSave()
    {
        if(IsEnabled == false)
            return;
        if (!_isEdited) return;
        var r = await DialogExtend.ShowSelectDialog("您确认要关闭编辑器吗？这个班级还没有保存!");
        if(r) 
            Save();
    }

    private async void DeleteSelfBtnOnClick(object? sender, RoutedEventArgs e)
    {
        var r = await DialogExtend.ShowSelectDialog("您确定要删除这个班级吗？", "是的", "不要", "删除警告");
        if (!r) return;
        _set.DeleteSelf();
        IsEnabled = false;
        _isEdited = false;
    }

    private async void RenameBtnOnClick(object? sender, RoutedEventArgs e)
    {
        _isEdited = true;
        var text = await DialogExtend.ShowInputDialog("重命名班级", "请输入班级的新名称:");
        if(text.Length <= 0) return;
        _set.Rename(text);
    }

    private void SaveBtnOnClick(object? sender, RoutedEventArgs e)
        => Save();

    private async void AddBtnOnClick(object? sender, RoutedEventArgs e)
    {
        _isEdited = true;
        var text = await DialogExtend.ShowInputDialog("添加成员", "请输入班级成员(可以多行输入)");
        if(text.Length <= 0) return;
        var items = text.Split("\n");
        
        _context.ContentItems.AddRange(items);
        _set.AddRange(items);
        
        UpdateListValue();
    }

    private void RemoveBtnOnClick(object? sender, RoutedEventArgs e)
    {
        _isEdited = true;
        if(TicketDisplay.SelectedItem is not string str) return;
        var r = _context.ContentItems.Remove(str);
        _set.Remove(str);
        
        if(!r) 
            DialogExtend.ShowErrorDialog("删除班级文件失败，请查看是否仍存在在列表中!", "删除失败");
        
        UpdateListValue();
    }

    private void UpdateListValue()
    {
        TicketDisplay.ItemsSource = null;
        TicketDisplay.Items.Clear();
        TicketDisplay.ItemsSource = _context.ContentItems;
    }

    public void OnKill()
    {
        RemoveBtn.Click -= RemoveBtnOnClick;
        AddBtn.Click -= AddBtnOnClick;
        SaveBtn.Click -= SaveBtnOnClick;
        RenameBtn.Click -= RenameBtnOnClick;
        DeleteSelfBtn.Click -= DeleteSelfBtnOnClick;
    }

    ~EditorPage() => OnKill();
}