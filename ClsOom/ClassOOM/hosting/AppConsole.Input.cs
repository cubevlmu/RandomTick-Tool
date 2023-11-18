namespace ClsOom.ClassOOM.hosting;

public partial class AppConsole
{
    public void PublicConsoleTask(Action<string> action)
    {
        if (CsTasks.Contains(action))
            return;
        CsTasks.Add(action);
        GC.Collect();
    }

    private async void ConsoleReaderTask()
    {
        string cs = await Console.In.ReadLineAsync();
        foreach(var action in CsTasks)
            action.Invoke(cs);
        ConsoleReaderTask();
    }

    private readonly List<Action<string>> CsTasks = new List<Action<string>>();
}