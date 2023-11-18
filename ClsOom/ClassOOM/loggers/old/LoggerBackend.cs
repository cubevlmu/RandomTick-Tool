using System.Collections.Concurrent;

namespace ClsOom.ClassOOM.loggers.old
{
    public readonly struct LogPrintTask
	{
		public LogPrintTask(string a, string c, LogType b)
		{ Text = a; Color = b; Text2 = c; }

		public readonly string Text;
		public readonly string Text2;
		public readonly LogType Color;
	}

	public sealed class LoggerBackend
	{
		public void AddLogTask(string part1, string part2, LogType color)
		{
			_tasks.Add(new LogPrintTask(part1, part2, color));
		}

		public void Start()
		{
			_ = Task.Run(PrintThread);
			GC.Collect();
		}

		private async Task PrintThread()
        {
            var task = _tasks.Take();
            await _stream.Write(task.Text2);
            _stream.WriteColor(task.Color.GetLogTypeConsoleColor());
            await _stream.Write(task.Color.GetLogTypeShortString());
            _stream.Reset();
            await _stream.Write(task.Text);
            await _stream.WriteLine();

            _ = Task.Run(PrintThread);
        }

		private LoggerBackend() { Start(); }

		private static readonly LoggerBackend Backend = new();
		public static LoggerBackend FetchBackEnd() => Backend;
		private readonly BlockingCollection<LogPrintTask> _tasks = new();
        private readonly IoStream _stream = IoStream.GetStream();
	}
}

