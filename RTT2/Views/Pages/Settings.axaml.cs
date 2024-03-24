using Avalonia.Controls;
using Avalonia.Interactivity;
using ClsOom.ClassOOM.il8n;
using RandomTick.Models;
using RandomTick.RandomTick.seat;
using RandomTick.RandomTick.ticket;

namespace RandomTick.Views.Pages;

public partial class Settings : Panel, IUiEventManage
{
    public Settings()
    {
        InitializeComponent();
        
        OnInit();
    }

    public void OnInit()
    {
        VSDevelop.IsCheckedChanged += VSDevelopOnIsCheckedChanged;
        VSDevelop.IsChecked = App.RttApp.Config.IsDebugMode;
        SaveLang.Click += SaveLangOnClick;
        CheatEntrance.DoubleTapped += CheatEntranceOnClick;

        UpdateLanguageSelector();
    }

    private int _cd;
    private async void CheatEntranceOnClick(object? sender, RoutedEventArgs e)
    {
        _cd++;
        if (_cd != 3) return;
        _cd = 0;
            
        if(!App.RttApp.Config.CheatMode) return;

        var ii = await DialogExtend.ShowComboDialog("选择模式", "选择一个模式", new[] { "抽签作弊", "排座位作弊" });
        switch (ii)
        {
            case -1:
                return;
            case 0:
            {
                var rr = await DialogExtend.ShowInputDialog("输入白名单", "每行输入一个名字，使用回车分割:");
                var lines = rr.Split('\n');
                var ec = new TicketCheatEncoder(lines);
                await ec.Begin();
                await ec.End();
                DialogExtend.ShowErrorDialog("pf文件生成成功", "成功");
                break;
            }
            case 1:
            {
                var wr = await DialogExtend.ShowInputDialog("输入白名单", "每行输入两个名字，用-分开，座位挨着的两个，使用回车分割: 例如: a-b ");
                var br = await DialogExtend.ShowInputDialog("输入黑名单", "每行输入两个名字，用-分开，座位分开的两个，使用回车分割: 例如: a-b ");

                var rr = new CheatEncoder(wr.Split('\n'), br.Split('\n'));
                await rr.End();
            
                DialogExtend.ShowErrorDialog("pf文件生成成功", "成功");
                break;
            }
        }
    }

    private void SaveLangOnClick(object? sender, RoutedEventArgs e)
    {
        var type = (Il8NType?)VSLanguages.SelectedItem ?? Il8NType.ZhCn;
        App.RttApp.Server.SetSelectedType(type);
        App.RttApp.Config.SelectedLanguage.Set(type);
        DialogExtend.ShowErrorDialog("虽然您修改了配置，但是需要重启程序才能生效", "✅成功!");
    }

    private void UpdateLanguageSelector()
    {
        App.RttApp.Server.GetExistTypes(out var types)
            .GetSelectedType(out var s);
        VSLanguages.ItemsSource = types;
        VSLanguages.SelectedIndex = types.IndexOf(s);
    }

    private void VSDevelopOnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var ck = VSDevelop.IsChecked ?? false;
        App.RttApp.Config.IsDebugMode.Set(ck);
    }

    public void OnKill()
    {
        
    }

    ~Settings()
    {
        OnKill();
    }
}