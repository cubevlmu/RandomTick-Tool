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

    [Il8N] public string? RemoveText;
    [Il8N] public string? AddText;
    [Il8N] public string? SaveText;
    [Il8N] public string? RenameText;
    [Il8N] public string? DeleteSelfText;

    [Il8N] public string? AddTickets;
    [Il8N] public string? AddTicketsTips;

    [Il8N("RandomTick.Dialogs.RemoveFailed")] 
    public string? RemoveFailed;
    [Il8N("RandomTick.Dialogs.DialogErrorTitle")] 
    public string? DialogErrorTitle;

    [Il8N] public string? RenameSet;
    [Il8N] public string? RenameSetTips;
    [Il8N] public string? NotSaveTips;
    
    [Il8N("RandomTick.Dialogs.DeleteMsg")] public string? DeleteMsg;
    [Il8N("RandomTick.Dialogs.DeleteYes")] public string? DeleteYes;
    [Il8N("RandomTick.Dialogs.DeleteNo")] public string? DeleteNo;
    [Il8N("RandomTick.Dialogs.DeleteTitle")] public string? DeleteTitle;
    
    [Il8N("RandomTick.Dialogs.SuccessContent")] public string? SaveSuccess;
    [Il8N("RandomTick.Dialogs.SuccessTitle")] public string? SuccessTitle;
    

    public void OnInit()
    {
        TicketDisplay.ItemsSource = _context.ContentItems;
        RemoveBtn.Click += RemoveBtnOnClick;
        RemoveBtn.Label = RemoveText;
        
        AddBtn.Click += AddBtnOnClick;
        AddBtn.Label = AddText;
        
        SaveBtn.Click += SaveBtnOnClick;
        SaveBtn.Label = SaveText;
        
        RenameBtn.Click += RenameBtnOnClick;
        RenameBtn.Label = RenameText;
        
        DeleteSelfBtn.Click += DeleteSelfBtnOnClick;
        DeleteSelfBtn.Label = DeleteSelfText;
    }
    
    public EditorPage(TicketSet set)
    {
        App.RTTApp.Server.UpdateFields(this);
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
        DialogExtend.ShowErrorDialog(SaveSuccess!, SuccessTitle!);
    }

    public async void CallSave()
    {
        if(IsEnabled == false)
            return;
        if (!_isEdited) return;
        var r = await DialogExtend.ShowSelectDialog(NotSaveTips!);
        if(r) 
            Save();
    }

    private async void DeleteSelfBtnOnClick(object? sender, RoutedEventArgs e)
    {
        var r = await DialogExtend.ShowSelectDialog(DeleteMsg!, DeleteYes!, DeleteNo!, DeleteTitle!);
        if (!r) return;
        _set.DeleteSelf();
        IsEnabled = false;
        _isEdited = false;
    }

    private async void RenameBtnOnClick(object? sender, RoutedEventArgs e)
    {
        _isEdited = true;
        var text = await DialogExtend.ShowInputDialog(RenameSet!, RenameSetTips!);
        if(text.Length <= 0) return;
        _set.Rename(text);
    }

    private void SaveBtnOnClick(object? sender, RoutedEventArgs e)
        => Save();

    private async void AddBtnOnClick(object? sender, RoutedEventArgs e)
    {
        _isEdited = true;
        var text = await DialogExtend.ShowInputDialog(AddTickets!, AddTicketsTips!);
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
            DialogExtend.ShowErrorDialog(RemoveFailed!, DialogErrorTitle!);
        
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