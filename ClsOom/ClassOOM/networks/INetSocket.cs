namespace ClsOom.ClassOOM.networks;

public interface INetSocket
{
    
}

public interface ITcpSocket : IDisposable, INetSocket
{
    public bool Open(string address, int port, bool isServerSocket);
    public bool Read(ref byte[]? readResult);
    public bool ReadWithBuffer(ref byte[]? readResult);
    public bool ReadWithSpanBuffer(ref byte[]? readResult);
    public bool ReadWithMultiThread(ref byte[]? result);
    public int Send(ref byte[] data);
    public Task<int> SendWithMultiThread(byte[] data);
    public void UpdateUnitBufferSize(int size);
    public void Close();
    public Task<ITcpSocket?> AcceptIncomingSocket();
}