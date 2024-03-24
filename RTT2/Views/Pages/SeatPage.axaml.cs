using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ClsOom.ClassOOM.loggers;
using FluentAvalonia.UI.Controls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RandomTick.Models;
using RandomTick.RandomTick.seat;
using RandomTick.RandomTick.services;
using RandomTick.ViewModels;
using FontFamily = Avalonia.Media.FontFamily;

namespace RandomTick.Views.Pages;

public partial class SeatPage : Panel, IUiEventManage
{
    private readonly TickSetService _service;
    private SeatBuilder? _builder;
    private readonly ILogger _logger;

    private bool _isInit;
    private readonly FontFamily _resource;
    private bool _isGen;

    internal class SeatPageItemMenu : FAMenuFlyout
    {
        private readonly int _x;
        private readonly int _y;
        private readonly SeatPage _page;
        private readonly InfoBar _parent;

        public SeatPageItemMenu(int x, int y, SeatPage page, InfoBar parent)
        {
            _x = x;
            _y = y;
            _page = page;
            _parent = parent;


            var emptySetBtn = new MenuFlyoutItem
            {
                Text = "设置为空座位",
                IconSource = new FontIconSource()
                {
                    FontFamily = _page._resource,
                    Glyph = "\uea18"
                }
            };
            var preSetBtn = new MenuFlyoutItem()
            {
                Text = "预先指定成员",
                IconSource = new FontIconSource()
                {
                    FontFamily = _page._resource,
                    Glyph = "\ue748"
                }
            };
            var cancelPreSetBtn = new MenuFlyoutItem()
            {
                Text = "还原该座位设置",
                IconSource = new FontIconSource()
                {
                    FontFamily = _page._resource,
                    Glyph = "\ue777"
                }
            };
        
            preSetBtn.Click += PreSetBtnOnClick;
            emptySetBtn.Click += EmptySetBtnOnClick;
            cancelPreSetBtn.Click += CancelPreSetBtnOnClick;

            Items.Add(emptySetBtn);
            Items.Add(preSetBtn);
            Items.Add(cancelPreSetBtn);
        }

        private void CancelPreSetBtnOnClick(object? sender, RoutedEventArgs e)
        {
            if (_page._builder == null)
            {
                DialogExtend.ShowErrorDialog("请先初始化!");
                return;
            }
            
            _page._builder
                .UnsetThis(_x, _y);

            _parent.Message = "";
        }

        private void EmptySetBtnOnClick(object? sender, RoutedEventArgs e)
        {
            if (_page._builder == null)
            {
                DialogExtend.ShowErrorDialog("请先初始化!");
                return;
            }
            
            _page._builder
                .SetPlaceholder(_x, _y);
            _parent.Message = App.RttApp.Config.IsDebugMode ? "[PH]" : " ";
        }

        private async void PreSetBtnOnClick(object? sender, RoutedEventArgs e)
        {
            if (_page._builder == null)
            {
                DialogExtend.ShowErrorDialog("请先初始化!");
                return;
            }

            var cc = _page._builder.GetCurrent();
            var idx = await DialogExtend.ShowComboDialog("选择成员", "从班级中选择一个成员来定下这里吧!", cc, "确认", "取消");
            if(idx == -1) return;
            
            var vv = cc[idx];
            
            _page._builder
                .SetPreFill(_x, _y, idx);
            _parent.Message = App.RttApp.Config.IsDebugMode ? vv + "[P]" : vv;
        }

        public Task Display()
        {
            if (_page._isGen)
                return Task.CompletedTask;
            
            _parent.ContextFlyout = this;
            _parent.ContextFlyout.ShowAt(_parent);

            return Task.CompletedTask;
        }
    }


    public SeatPage()
    {
        var model = new SeatPageViewModel();
        DataContext = model;
        
        _resource = App.Current.FindResource("FluentIcon") as FontFamily ?? throw new Exception("Empty Font Resource");
        
        App.RttApp.Server
            .GetService("TickSetService", out TickSetService? v)
            .GetLogger("Seat", out _logger);
        _service = v ?? throw new Exception("Error To Load Data");
        
        InitializeComponent();

        OnInit();
    }

    private async void InputElement_OnTapped(object? sender, TappedEventArgs _)
    {
        if(sender is not InfoBar bar) return;
        var set = _service[_service.GetSetsNames[0]];
        if(set == null)
            return;
        var tt = (string?)bar.Tag;
        if(tt == null) 
            return; 
        // _logger.Info($"Tag {tt}");
        var parts = tt.Split('/');

        if(!int.TryParse(parts[0], out var x)) return;
        if(!int.TryParse(parts[1], out var y)) return;

        SeatPageItemMenu menu = new(x - 1, y - 1, this, bar);
        await menu.Display();
    }

    public void OnInit()
    {
        Setup.Click += SetupOnClick;
        Gen.Click += GenOnClick;
        
        SaveXls.Click += SaveXlsOnClick;
    }

    private async void SaveXlsOnClick(object? sender, RoutedEventArgs e)
    {
        if (!_isGen)
        {
            DialogExtend.ShowErrorDialog("生成xls表格之前需要先生成座位表!");
            return;
        }

        if (_builder == null)
            return;

        var mm = _builder.GetMap();
        
        IWorkbook workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("座位表");

        for (var j = 0; j < _builder.GetHeight(); j++)
        {
            var row = sheet.CreateRow(j);
            for (var i = 0; i < _builder.GetWidth(); i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(mm[i, j] == SeatBuilder.Placeholder ? " " : mm[i, j]);
            }
        }
        
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;
        
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "保存表格文件",
            FileTypeChoices = new []{ new FilePickerFileType("xlsx") },
            DefaultExtension = "xlsx"
        });

        if (file is null) return;
        await using var stream = await file.OpenWriteAsync();
        workbook.Write(stream);
        stream.Close();
        
        DialogExtend.ShowErrorDialog("保存成功!请查看存储后的表格文件", "保存成功!");
    }

    private void GenOnClick(object? sender, RoutedEventArgs e)
    {
        if (!_isInit || _builder == null)
        {
            DialogExtend.ShowErrorDialog("请先初始化视图 :(");
            return;
        }

        if (_isGen)
        {
            DialogExtend.ShowErrorDialog("已经生成过依次了，请重新生成视图来继续！");
            return;
        }
        
        _builder
            .PreCheatPlace()
            .Gen()
            .End();
        var mm = _builder.GetMap();

        Test.Items.Clear();

        for (var j = 0; j < _builder.GetHeight(); j++)
        {
            for (var i = 0; i < _builder.GetWidth(); i++)
            {
                Test.Items.Add(new SeatItem
                {
                    IndexText = $"{i + 1}/{j + 1}",
                    Name = mm[i, j] == SeatBuilder.Placeholder ? " " : mm[i, j],
                });
            }
        }

        _isGen = true;
    }

    private async void SetupOnClick(object? sender, RoutedEventArgs e)
    {
        if (_isInit || _builder != null)
        {
            Test.Items.Clear();
            _builder?.End();
            _builder = null;

            Setup.IsChecked = false;
        }

        var rr = await DialogExtend.ShowComboDialog("选择班级", "请选择一个班级来继续: ", _service.GetSetsNames, "确定");
        var names = _service.GetSetsNames;
        if (rr < 0 || rr > names.Length)
        {
            DialogExtend.ShowErrorDialog("错误的班级!");
            return;
        }
        
        var set = _service[names[rr]];
        if (set == null)
        {
            DialogExtend.ShowErrorDialog("错误的班级!");
            return;
        }

        var row = await DialogExtend.ShowNumberDialog("选择列数", "请选择视图列数(座位表列数):", 6, 0, "确认");
        if (row < 0)
        {
            DialogExtend.ShowErrorDialog("错误的列数!");
            return;
        }
        
        var column = await DialogExtend.ShowNumberDialog("选择行数", "请选择视图行数(座位表行数):", 7, 0,"确认");
        if (column < 0)
        {
            DialogExtend.ShowErrorDialog("错误的行数!");
            return;
        }

        _builder = new SeatBuilder(row, column);
        _builder.Begin(set);
        
        Test.Items.Clear();
        for (var j = 0; j < _builder.GetHeight(); j++)
        {
            for (var i = 0; i < _builder.GetWidth(); i++)
            {
                Test.Items.Add(new SeatItem
                {
                    IndexText = $"{i + 1}/{j + 1}",
                    Name = ""
                });
            }
        }

        _isGen = false;
        _isInit = true;
        Setup.IsChecked = true;
        
        DialogExtend.ShowErrorDialog("初始化成功", "成功!");
    }

    public void OnKill()
    {
        Setup.Click -= SetupOnClick;
        Gen.Click -= GenOnClick;
        
        SaveXls.Click -= SaveXlsOnClick;
    }

    ~SeatPage() => OnKill();
}