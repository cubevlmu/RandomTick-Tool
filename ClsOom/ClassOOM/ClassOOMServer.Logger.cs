using ClsOom.ClassOOM.loggers;
using ClsOom.ClassOOM.loggers.services;

namespace ClsOom.ClassOOM;

public partial class ClassOomServer
{
    protected ClassOomServer BeginLogger()
    {
        _bufferedLoggers.Clear();
        _service = new LogRecordService(this, "logs");
        return this;
    }
    
    public ClassOomServer GetLogger(string name, out ILogger logger)
    {
        if (_bufferedLoggers.TryGetValue(name, out var bufferedLogger))
        {
            logger = bufferedLogger;
            return this;
        }

        logger = new LoggerBackend(
            _service ?? throw new Exception("Logger Module Not Init"), 
            name
        );
        _bufferedLoggers.Add(name, logger);
        return this;
    }

    public ClassOomServer GetLogger(object instance, out ILogger logger)
    {
        var tp = instance.GetType();
        
        if (_bufferedLoggers.TryGetValue(tp.Name, out var bufferedLogger))
        {
            logger = bufferedLogger;
            return this;
        }
        
        logger = new LoggerBackend(
            _service ?? throw new Exception("Logger Module Not Init"), 
            tp.Name
        );
        return this;
    }
    
    
    public ClassOomServer EndLogger()
    {
        _bufferedLoggers.Clear();
        _service = null;
        return this;
    }

    private LogRecordService? _service;
    private readonly Dictionary<string, ILogger> _bufferedLoggers = new();
}