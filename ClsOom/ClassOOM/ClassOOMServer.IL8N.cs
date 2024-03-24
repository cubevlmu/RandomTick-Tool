using ClsOom.ClassOOM.il8n;

namespace ClsOom.ClassOOM;

public partial class ClassOomServer
{

    public ClassOomServer BeginIl8N(Il8NReader reader)
    {
        _reader = reader;
        _loadedIl8NFiles.Clear();
        _delegatedFields.Clear();
        
        return this;
    }

    public ClassOomServer LoadAllIl8N()
    {
        if (_reader == null) return this;
        
        GetDataSubDir("il8n", out var dir);
        var info = new DirectoryInfo(dir);
        var fs = info.GetFiles();
        foreach (var file in fs)
        {
            if (file is not { Exists: true, Extension: ".lang" }) continue;
            var f = new Il8NFile(_reader, file.Name, this);
            _loadedIl8NFiles.Add(f);
        }

        return this;
    }

    
    [Obsolete("Not Support Anymore")]
    public ClassOomServer NewIl8NFile(string name)
    {
        if (_reader == null) return this;
        var file = new Il8NFile(_reader, name, this);
        _loadedIl8NFiles.Add(file);
        return this;
    }

    public ClassOomServer AddIl8NField(Il8NField f)
    {
        _delegatedFields.Add(f);
        return this;
    }

    public ClassOomServer SetSelectedType(Il8NType t)
    {
        _selectedType = t;
        return this;
    }

    public ClassOomServer GetSelectedType(out Il8NType t)
    {
        t = _selectedType;
        return this;
    }

    public ClassOomServer GetExistTypes(out IList<Il8NType> types)
    {
        var t = types = new List<Il8NType>();
        _loadedIl8NFiles.ForEach(item => t.Add(item.Info.Type));
        return this;
    }

    public ClassOomServer GetTranslate(string key, out string value)
    {
        var lang = _loadedIl8NFiles
            .Where(t => t.Info.Type == _selectedType);
        
        value = key;
        
        foreach (var file in lang ?? throw new Exception("No Lang File Selected"))
        {
            if(!file.Contains(key)) continue;
            var v = file[key];
            value = v;
        }

        return this;
    }

    public ClassOomServer UpdateFields(object classInstance)
    {
        var tp = classInstance.GetType();
        var fields = tp.GetFields();
        
        var lang = _loadedIl8NFiles
            .Where(t => t.Info.Type == _selectedType);

        foreach (var fieldInfo in fields)
        {
            var isSet = false;
            var attributes = fieldInfo.GetCustomAttributes(true);
            var il8NAttribute = attributes.GetAttribute<Il8NAttribute>();
            if(il8NAttribute == null) continue;
            var key = il8NAttribute.Key.Length == 0 ? $"{tp.FullName}.{fieldInfo.Name}" : il8NAttribute.Key;
            
            foreach (var file in lang ?? throw new Exception("No Lang File Selected"))
            {
                if(!file.Contains(key)) continue;
                var v = file[key];
                fieldInfo.SetValue(classInstance, v);
                isSet = true;
            }

            if (isSet) continue;
            GetLogger("RTT", out var logger);
            logger.Warn($"[IL8N] Key {key} Field {fieldInfo.Name} From {tp.FullName} Has No Translate!");
            fieldInfo.SetValue(classInstance, key);
        }

        return this;
    }

    [Obsolete("No Support Anymore")]
    public Task<ClassOomServer> UpdateAllFields()
    {
        _il8NUpdateLock = true;
        var res = _loadedIl8NFiles
            .Where(t => t.Info.Type == _selectedType);
        
        _delegatedFields.ForEach(item =>
        {
            foreach (var r in res)
                item.OnFile(r);
        });
        _il8NUpdateLock = false;
        return Task.FromResult(this);
    }

    public ClassOomServer WaitForLock()
    {
        while (_il8NUpdateLock)
        {
            Thread.Sleep(300);
        }

        return this;
    }

    public ClassOomServer EndIl8N()
    {
        _loadedIl8NFiles.ForEach(item => item.Save());
        _loadedIl8NFiles.Clear();
        _delegatedFields.Clear();
        _reader = null;
        return this;
    }

    private readonly List<Il8NField> _delegatedFields = new();
    private readonly List<Il8NFile> _loadedIl8NFiles = new();
    private Il8NReader? _reader;
    private bool _il8NUpdateLock;
    private Il8NType _selectedType = Il8NType.EnUs;
}