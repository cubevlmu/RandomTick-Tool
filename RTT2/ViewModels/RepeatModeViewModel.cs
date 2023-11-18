namespace RandomTick.ViewModels;

public class RepeatModeViewModel : ViewModelBase
{
    public string[] Name { get; }

    public RepeatModeViewModel(string[] names)
    {
        Name = names;
    }

    public RepeatModeViewModel()
    {
        Name = new[] { "Empty" };
    }
}