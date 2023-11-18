using System.Collections.Generic;
using RandomTick.Views.Pages;
using RandomTick.Views.Pages.Editor;

namespace RandomTick.ViewModels;

public class TagPage
{
    public string Title { get; init; } = "Page";
    public bool Closeable { get; init; }
    public object? Content { get; init; }
}

public class TagEditorViewModel : ViewModelBase
{
    public List<TagPage> TabPages { get; }

    public TagEditorViewModel()
    {
        TabPages = new List<TagPage>();
    }
    
    public TagEditorViewModel(TagEditor e)
    {
        TabPages = new List<TagPage>
        {
            new()
            {
                Title = "Home",
                Closeable = false,
                Content = new EditorHomePage(e)
            }
        };
    }

    public void OpenNewPage(string title, EditorPage page)
    {
        TabPages.Add(new TagPage
        {
            Title = title,
            Closeable = true,
            Content = page
        });
        
    }
}