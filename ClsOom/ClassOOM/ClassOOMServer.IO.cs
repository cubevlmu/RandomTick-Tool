namespace ClsOom.ClassOOM;

public partial class ClassOomServer
{
    protected ClassOomServer BeginIo()
    {
        _ = CheckUpDataDir();
        return this;
    }

    public ClassOomServer CheckUpDataDir()
    {
        if (!Directory.Exists($"ClassOOM//{_app.AppName}"))
            Directory.CreateDirectory($"ClassOOM//{_app.AppName}");
        return this;
    }
    
    public ClassOomServer GetDataDir(out string dir)
    {
        _ = CheckUpDataDir();
        dir = $"ClassOOM//{_app.AppName}";
        return this;
    }

    public ClassOomServer GetDataSubDir(string name, out string dir)
    {
        _ = CheckUpDataDir();
        dir = $"ClassOOM//{_app.AppName}//{name}";
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        return this;
        
    }

    public ClassOomServer ReadFile(string name, out string str)
    {
        _ = GetDataDir(out var dir);
        str = !File.Exists($"{dir}//{name}") ? 
               "" : 
               File.ReadAllText($"{dir}//{name}");
        return this;
    }

    public ClassOomServer CopyFileTo(string path, string name)
    {
        _ = GetDataDir(out var dir);
        File.Copy(path, $"{dir}//{name}");
        return this;
    }

    public ClassOomServer IsExist(string name, out bool result)
    {
        _ = GetDataDir(out var dir);
        result = File.Exists(name);
        return this;
    }
}