using System.Threading.Tasks;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace RandomTick.Models;

public static class DialogExtend
{
    public static async Task<bool> ShowSelectDialog(string content, string btn1 = "Yes", string btn2 = "No", string title = "❓Question")
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
    
    public static async Task<string> ShowInputDialog(string title,  string tips, string submitText = "Submit", string closeBtn = "Cancel", string watermark = "Input Somethings...")
    {
        var box = new TextBox
        {
            Watermark = watermark,
            AcceptsReturn = true
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