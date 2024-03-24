using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using FluentAvalonia.UI.Controls;

namespace RandomTick.Models;

public static class DialogExtend
{
    public static async Task<bool> ShowSelectDialog(string content, string btn1 = "Yes", string btn2 = "No",
        string title = "❓Question")
    {
        var dialog = new ContentDialog
        {
            Title = title,
            PrimaryButtonText = btn1,
            SecondaryButtonText = btn2,
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };

        var result = await dialog.ShowAsync();
        return result == ContentDialogResult.Primary;
    }
    
    public static async Task<int> ShowNumberDialog(
        string title, string hint,
        int defaultNum = 0,
        int minimum = 0,
        string submitText = "submit",
        string closeBtn = "close"
    )
    {
        var box = new NumberBox
        {
            Minimum = minimum,
            Value = defaultNum
        };
        var dialog = new ContentDialog
        {
            Title = title,
            PrimaryButtonText = submitText,
            CloseButtonText = closeBtn,
            DefaultButton = ContentDialogButton.Primary,
            Content = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = hint
                    },
                    box
                },
                Spacing = 10,
                MinWidth = 400
            }
        };

        var r = await dialog.ShowAsync();
        return r == ContentDialogResult.Primary ? (int)box.Value : -1;
    }

    public static async Task<int> ShowComboDialog(
        string title, string hint,
        IEnumerable<string> contents,
        string submitText = "submit",
        string closeBtn = "close"
    ) {
        var box = new ComboBox()
        {
            ItemsSource = contents
        };
        var dialog = new ContentDialog
        {
            Title = title,
            PrimaryButtonText = submitText,
            CloseButtonText = closeBtn,
            DefaultButton = ContentDialogButton.Primary,
            Content = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = hint
                    },
                    box
                },
                Spacing = 10,
                MinWidth = 400
            }
        };

        var r = await dialog.ShowAsync();
        return r == ContentDialogResult.Primary ? box.SelectedIndex : -1;
    }

    public static async Task<string> ShowInputDialog(string title, string tips, string submitText = "Submit",
        string closeBtn = "Cancel", string watermark = "Input Somethings...")
    {
        var box = new TextBox
        {
            Watermark = watermark,
            AcceptsReturn = true,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        var dialog = new ContentDialog
        {
            Title = title,
            PrimaryButtonText = submitText,
            CloseButtonText = closeBtn,
            DefaultButton = ContentDialogButton.Primary,
            Content = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Children =
                {
                    new TextBlock
                    {
                        Text = tips
                    },
                    box
                },
                Spacing = 10,
                MinWidth = 400
            }
        };

        var r = await dialog.ShowAsync();
        return r == ContentDialogResult.Primary ? box.Text ?? "" : "";
    }

    public static async void ShowErrorDialog(
        string content,
        string title = "Error",
        string buttonText = "Close"
    )
    {
        var dialog = new ContentDialog
        {
            Title = title,
            PrimaryButtonText = buttonText,
            Content = content
        };
        await dialog.ShowAsync();
    }
}