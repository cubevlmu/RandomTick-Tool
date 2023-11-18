namespace ClsOom.ClassOOM.config;

public interface IConfigBackend
{
    IDictionary<string, string> ReadAll(string path);
    bool SaveAll(string path, IDictionary<string, string> v);
    
    string ExtensionType { get; }
}