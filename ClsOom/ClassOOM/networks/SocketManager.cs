namespace ClsOom.ClassOOM.networks;

public class SocketManager
{
    public static ITcpSocket CreateSocket() => NetSocket.CreateSocket();
    
    private readonly Dictionary<string, ITcpSocket> _tcpSockets = new();
}