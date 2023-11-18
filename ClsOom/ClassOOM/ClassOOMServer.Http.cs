using System.Net;
using System.Text;

namespace ClsOom.ClassOOM;

public partial class ClassOomServer
{
    public ClassOomServer HttpGet(string path, out bool result, out string data)
    {
        result = false;
        data = "";

        var r = _wc.DownloadData(path);
        if (r.Length > 0) result = true;
        data = Encoding.UTF8.GetString(r);

        return this;
    }

    private readonly WebClient _wc = new();
}