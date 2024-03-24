using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI;

namespace RandomTick.ViewModels;

public struct SeatItem
{
    public string Name { get; init; }
    public string IndexText { get; init; }
    public object Self { get; }
    public int W { get; init; }
    public int H { get; init; }

    public SeatItem(string name, string index, int w, int h)
    {
        Name = name;
        IndexText = index;
        Self = this;
        W = w;
        H = h;
    }
}

public class SeatPageViewModel : ViewModelBase
{
    public ICommand ItemTapCommand { get; private set; }
    public SeatItem SelectedItem { get; set; }

    public int ItemWidth { get; set; } = 200;

    public SeatPageViewModel()
    {
        ItemTapCommand = ReactiveCommand.Create(() =>
        {
            
        });
    }
}