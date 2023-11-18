using ClsOom.ClassOOM.il8n;

namespace RandomTick.ViewModels;

public class HomePageViewModel : ViewModelBase
{
    public string[] Name { get; }

    public HomePageViewModel(string[] names)
    {
        Name = names;
    }

    public HomePageViewModel()
    {
        Name = new[] { "Empty" };
    }
}