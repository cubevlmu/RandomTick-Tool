namespace ClsOom.ClassOOM;

public interface IApp
{
    void OnLoad(ClassOomServer server);
    void OnKill(ClassOomServer server);

    ClassOomServer Server { get; }
    
    string AppName { get; }
    string AppId { get; }
    Version AppVersion { get; }
}