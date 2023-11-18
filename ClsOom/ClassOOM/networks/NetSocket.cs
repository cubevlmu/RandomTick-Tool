using System.Net;
using System.Net.Sockets;

namespace ClsOom.ClassOOM.networks;

internal sealed class NetSocket : ITcpSocket
{
    internal static ITcpSocket CreateSocket(SocketTypes type = SocketTypes.Tcp)
        => new NetSocket(type);
    
    private NetSocket(SocketTypes type = SocketTypes.Tcp)
    {
        _socket = new Socket(SocketType.Stream, type == SocketTypes.Tcp ? ProtocolType.Tcp : ProtocolType.Udp);
        _listener = null;
    }

    private NetSocket(Socket socket)
    {
        _socket = socket;
    }

    public bool Open(string address, int port, bool openServer)
    {
        _openedServer = openServer;
        if(_openedServer)
        {
            return OpenServer(address, port);
        } else
        {
            try
            {
                _socket.Connect(GetAddressFromStr(address), port);
                return _socket.Connected;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public async Task<ITcpSocket?> AcceptIncomingSocket()
    {
        if (!_openedServer)
            return null;
        if (_listener == null)
            throw new Exception("Socket Not Opened!");
        try
        {
            var skt = await _listener.AcceptSocketAsync();
            return skt.Connected ? new NetSocket(skt) : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    
    public bool Read(ref byte[]? result)
    {
        var res = new byte[_singleBufferSize];
        var times = 0;
        var minibuffer = _singleBufferSize;

        while (true)
        {
            var data = new byte[minibuffer];
            var size = _socket.Receive(data);
            if (size > 0)
            {
                var tmp = new byte[res.Length + size];
                Array.Copy(res, tmp, res.Length);
                Array.Copy(data, 0, tmp, res.Length, size);
                Array.Clear(data);
                
                Array.Clear(res);
                res = new byte[tmp.Length];
                Array.Clear(tmp);

                times++;
                if (times > 0 && times % 5 == 0)
                    minibuffer += _singleBufferSize;
            }
            else if (size == -1)
                return false;
            else
                break;
        }
        result = res;
        GC.Collect();
        return true; //TODO
    }

    public bool ReadWithBuffer(ref byte[]? result)
    {
        using (MemoryStream memoryStream = new())
        {

            var times = 0;
            var nowBufferSize = _singleBufferSize;

            while (true)
            {
                var data = new byte[nowBufferSize];
                var size = _socket.Receive(data);
                if (size > 0)
                {
                    memoryStream.Write(data, 0, size);

                    times++;
                    if (times > 0 && times % 5 == 0)
                        nowBufferSize += _singleBufferSize;
                }
                else if (size == -1)
                    return false;
                else
                    break;
            }

            result = memoryStream.ToArray();
            memoryStream.Close();
            memoryStream.Dispose();
        }

        GC.Collect();
        return true; // TODO
    }

    public bool ReadWithSpanBuffer(ref byte[]? result)
    {
        using (MemoryStream memoryStream = new())
        {

            var times = 0;
            var nowBufferSize = _singleBufferSize;
            Span<byte> data = stackalloc byte[nowBufferSize];

            while (true)
            {
                var size = _socket.Receive(data);
                if (size > 0)
                {
                    memoryStream.Write(data.ToArray(), 0, size);

                    times++;
                    if (times > 0 && times % 5 == 0)
                        nowBufferSize += _singleBufferSize;
                    data.Clear();
                }
                else if (size == -1)
                    return false;
                else
                    break;
            }

            result = memoryStream.ToArray();
            memoryStream.Close();
            memoryStream.Dispose();
        }

        GC.Collect();
        return true; // TODO
    }

    public bool ReadWithMultiThread(ref byte[]? result)
    {
        var receivedData = new List<byte[]>();
        var receiveLock = new object();
        var readCompleted = false;

        var receiveThread = new Thread(() =>
        {
            var nowBufferSize = _singleBufferSize;
            var buffer = new byte[nowBufferSize];

            while (true)
            {
                var size = _socket.Receive(buffer);
                if (size > 0)
                {
                    lock (receiveLock)
                    {
                        var data = new byte[size];
                        Array.Copy(buffer, 0, data, 0, size);
                        receivedData.Add(data);
                    }

                    if (receivedData.Count % 5 == 0)
                        nowBufferSize += _singleBufferSize;
                }
                else if (size == -1)
                {
                    lock (receiveLock)
                    {
                        readCompleted = true;
                    }
                    break;
                }
                else
                {
                    break;
                }
            }
        });

        receiveThread.Start();

        receiveThread.Join();

        if (!readCompleted) return false;
        {
            var totalSize = receivedData.Sum(data => data.Length);
            var mergedResult = new byte[totalSize];
            var offset = 0;

            foreach (var data in receivedData)
            {
                Array.Copy(data, 0, mergedResult, offset, data.Length);
                offset += data.Length;
            }

            result = mergedResult;
            return true;
        }

    }


    public int Send(ref byte[] data)
    {
        return _socket.Send(data);
    }

    public Task<int> SendWithMultiThread(byte[] data)
    {
        var totalSent = 0;
        var remainingBytes = data.Length;

        var tcs = new TaskCompletionSource<int>();

        void SendCallback(IAsyncResult ar)
        {
            try
            {
                int sent = _socket.EndSend(ar);

                if (sent <= 0)
                {
                    tcs.SetResult(totalSent);
                    return;
                }

                totalSent += sent;
                remainingBytes -= sent;

                if (remainingBytes > 0)
                {
                    var bytesToSend = Math.Min(remainingBytes, _singleBufferSize);
                    _socket.BeginSend(data, totalSent, bytesToSend, SocketFlags.None, SendCallback, null);
                }
                else
                {
                    tcs.SetResult(totalSent);
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }

        int bytesToSendFirst = Math.Min(remainingBytes, _singleBufferSize);
        _socket.BeginSend(data, 0, bytesToSendFirst, SocketFlags.None, SendCallback, null);

        return tcs.Task;
    }
    
    private bool OpenServer(string address, int port)
    {
        try
        {
            _listener = new TcpListener(GetAddressFromStr(address), port);
            _listener.Start();
            return true;
        } catch(Exception)
        { return false; }
    }

    private static IPAddress GetAddressFromStr(string address)
    {
        try
        {
            return address is {Length: > 0} ? IPAddress.Parse(address) : IPAddress.Any;
        } catch(Exception)
        {
            return IPAddress.Any;
        }
    }

    public void Close()
    {
        _socket.Close();
        _listener?.Stop();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(_socket);
    }

    public void UpdateUnitBufferSize(int size)
    {
        _singleBufferSize = size;
        Console.WriteLine("Updated Single Buffer Base Size : " + size);
    }

    public ConnectionInfo GetSocketInfo()
    {
        ConnectionInfo info = new();
        //TODO
        return info;
    }

    private int _singleBufferSize = 8192;
    private bool _openedServer;
    private readonly Socket _socket;
    private TcpListener? _listener;
}