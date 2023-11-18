using System.Net;

namespace ClsOom.ClassOOM.networks;

public enum SocketTypes
{ Tcp, Udp }

public struct ConnectionInfo
{
    public IPAddress RemoteAddress;
    public int RemotePort;

    public IPAddress LocalAddress;
    public int LocalPort;
}
