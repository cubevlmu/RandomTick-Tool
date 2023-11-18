namespace ClsOom.ClassOOM.il8n;

public struct Il8NField
{
    private readonly string _key;
    private string _t;

    public Il8NField(string key)
    {
        _key = key;
        _t = "[MissingTranslate]";

        var server = ClassOomServer.GetStaticServer();
        server.AddIl8NField(this);
    }

    public void OnFile(Il8NFile file)
    {
        var r = file[_key];
        Set(r);
    }

    public void Set(string str) => _t = str;
    public static implicit operator string(Il8NField f) => f._t;
    public override string ToString() => _t;
}