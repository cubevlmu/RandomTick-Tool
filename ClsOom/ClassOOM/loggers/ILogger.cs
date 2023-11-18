namespace ClsOom.ClassOOM.loggers;

public interface ILogger
{
    void Info(object msg);
    void Info(string template, params object[] msg);
    void Info(params object[] msg);
    
    
    void Error(object msg);
    void Error(string template, params object[] msg);
    void Error(params object[] msg);
    void Error(Exception e);
    
    
    void Warn(object msg);
    void Warn(string template, params object[] msg);
    void Warn(params object[] msg);
    void Warn(Exception e);
    
    
    void Debug(object msg);
    void Debug(string template, params object[] msg);
    void Debug(params object[] msg);
}