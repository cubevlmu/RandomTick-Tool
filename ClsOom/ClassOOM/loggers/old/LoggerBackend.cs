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

		private Task PrintThread()
        {
            var task = _tasks.Take();
            lock (_stream)
            {
	            _stream.Write(task.Text2);
	            _stream.WriteColor(task.Color.GetLogTypeConsoleColor());
	            _stream.Write(task.Color.GetLogTypeShortString());
	            _stream.Reset();  
	            _stream.Write(task.Text);
	            _stream.WriteLine();
            }

            return Task.Run(PrintThread);
        }

		private LoggerBackend() { Start(); }

		private static readonly LoggerBackend Backend = new();
		public static LoggerBackend FetchBackEnd() => Backend;
		private readonly BlockingCollection<LogPrintTask> _tasks = new();
        private readonly IoStream _stream = IoStream.GetStream();
	}
}

