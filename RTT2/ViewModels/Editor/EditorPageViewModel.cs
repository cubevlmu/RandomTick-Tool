using System.Collections.Generic;
using RandomTick.RandomTick.models;

namespace RandomTick.ViewModels.Editor;

public class EditorPageViewModel : ViewModelBase
{
    public List<string> ContentItems { get; set; }

    public EditorPageViewModel(TicketSet set)
    {
        ContentItems = new List<string>(set.Get());
    }
}