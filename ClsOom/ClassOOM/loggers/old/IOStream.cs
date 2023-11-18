using System.Collections;

namespace ClsOom.ClassOOM.loggers.old;

public class IoStream
{
    public async Task<IoStream> Write(string text)
    {
        await _output.WriteAsync(text);
        return this;
    }

    public IoStream WriteColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
        return this;
    }

    public IoStream Reset()
    {
        Console.ResetColor();
        return this;
    }

    public async Task<IoStream> WriteLine()
    {
        await _output.WriteLineAsync();
        return this;
    }

    public async Task<IoStream> Flush()
    {
        await _output.FlushAsync();
        return this;
    }

    private static readonly IoStream _stream = new();
    public static IoStream GetStream() => _stream;
    private IoStream()
    {
        var backUp = Console.Out;
        _output = backUp;
    }

    private readonly TextWriter _output = Console.Out;
}
//
// public class FakeTextOutput : TextWriter
// {
//     public override Encoding Encoding => Encoding.UTF8;
//
//     public override void Flush()
//     {
//         _logger.Info(_builder.ToString());
//         _builder.Clear();
//     }
//     public override Task FlushAsync()
//     {
//         _logger.Info(_builder.ToString());
//         _builder.Clear();
//         return Task.CompletedTask;
//     }
//
//     public override string? ToString() => "FakeTextWrite For SystemIO";
//
//     public override void Write(bool value) => _logger.Info("{0}", value);
//     public override void Write(char value) => _logger.Info("{0}", value);
//
//     public override void Write(char[]? buffer)
//     {
//         var result = buffer?.ToString();
//         _logger.Info(result ?? "");
//     }
//     public override void Write(char[] buffer, int index, int count)
//     {
//         var str = new string(buffer, index, count);
//         _logger.Info(str);
//     }
//
//     public override void Write(decimal value) => _logger.Info("{0}", value);
//     public override void Write(double value) => _logger.Info("{0}", value);
//     public override void Write(int value) => _logger.Info("{0}", value);
//     public override void Write(long value) => _logger.Info("{0}", value);
//     public override void Write(object? value) => _logger.Info("{0}", value);
//
//     public override void Write(ReadOnlySpan<char> buffer) => _logger.Info("{0}", buffer.ToString());
//     public override void Write(float value) => _logger.Info("{0}", value);
//     public override void Write(string? value) => _logger.Info("{0}", value);
//
//     public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0) => _logger.Info(string.Format(format, arg0));
//     public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
//          => _logger.Info(string.Format(format, arg0, arg1));
//     public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
//          => _logger.Info(string.Format(format, arg0, arg1, arg2));
//     public override void Write([StringSyntax("CompositeFormat")] string format, params object?[] arg)
//          => _logger.Info(string.Format(format, arg));
//
//     public override void Write(StringBuilder? value) => _logger.Info("{0}", value);
//     public override void Write(uint value) => _logger.Info("{0}", value);
//     public override void Write(ulong value) => _logger.Info("{0}", value);
//
//     public override Task WriteAsync(char value)
//     {
//         _logger.Info("{0}", value);
//         return Task.CompletedTask;
//     }
//
//     public override Task WriteAsync(char[] buffer, int index, int count)
//     {
//         _logger.Info(new string(buffer, index, count));
//         return Task.CompletedTask;
//     }
//
//     public override Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
//     {
//         _logger.Info("{0}", buffer.ToString());
//         return Task.CompletedTask;
//     }
//
//     public override Task WriteAsync(string? value)
//     {
//         _logger.Info("{0}", value ?? "");
//         return Task.CompletedTask;
//     }
//
//     public override Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default)
//     {
//         _logger.Info("{0}", value?.ToString()!);
//         return Task.CompletedTask;
//     }
//
//     public override void WriteLine() => _logger.Info("");
//     public override void WriteLine(bool value) => _logger.Info("{0}", value);
//     public override void WriteLine(char value) => _logger.Info("{0}", value);
//     public override void WriteLine(char[]? buffer) => _logger.Info(new string(buffer));
//     public override void WriteLine(char[] buffer, int index, int count)
//         => _logger.Info(new string(buffer, index, count));
//     public override void WriteLine(decimal value) => _logger.Info("{0}", value);
//     public override void WriteLine(double value) => _logger.Info("{0}", value);
//     public override void WriteLine(int value) => _logger.Info("{0}", value);
//     public override void WriteLine(long value) => _logger.Info("{0}", value);
//     public override void WriteLine(object? value) => _logger.Info("{0}", value ?? "");
//     public override void WriteLine(ReadOnlySpan<char> buffer) => _logger.Info("{0}", buffer.ToString());
//     public override void WriteLine(float value) => _logger.Info("{0}", value);
//     public override void WriteLine(string? value) => _logger.Info("{0}", value ?? "");
//
//     public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0)
//     {
//         base.WriteLine(format, arg0);
//     }
//
//     public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
//     {
//         base.WriteLine(format, arg0, arg1);
//     }
//
//     public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
//     {
//         base.WriteLine(format, arg0, arg1, arg2);
//     }
//
//     public override void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] arg)
//     {
//         base.WriteLine(format, arg);
//     }
//
//     private readonly BlockingCollection<string> _tasks = new();
//     private readonly StringBuilder _builder = new();
// }

public class InputIoStream : IEnumerator<string>, IEnumerator
{
    public void Start() => _ = Task.Run(GetNext);

    private async Task GetNext()
    {
        var text = await _stream.ReadToEndAsync();
        text ??= "";
        foreach (var item in _callbacks)
            item.Invoke(text);
        _received.Add(text);

        _ = Task.Run(GetNext);
    }

    public void AddTask(Action<string> back) => _callbacks.Add(back);

    public void Clear() => _received.Clear();
    public string[] GetReceived() => _received.ToArray();

    public bool MoveNext()
    {
        _index++;
        return _index < _received.Count;
    }

    public void Reset() => _index = 0;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public object Current => this;

    string IEnumerator<string>.Current => _received[_index];

    private readonly List<string> _received = new();
    private readonly List<Action<string>> _callbacks = new();
    private readonly TextReader _stream = Console.In;

    private InputIoStream() => GC.Collect();//Start();

    private static readonly InputIoStream Ios = new();
    public static InputIoStream GetInput() => Ios;

    private int _index = 0;
}

