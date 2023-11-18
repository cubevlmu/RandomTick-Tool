namespace ClsOom.ClassOOM.service;

public interface IService
{
    void OnLoad(ClassOomServer s);
    void OnKill(ClassOomServer s);

    object? GetFunction(string name);
    object? GetField(string name);
    
    public string Name { get; }
}