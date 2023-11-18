using System.Text;
using ClsOom.ClassOOM.model.Tools;

namespace ClsOom.ClassOOM.model;

public partial class PacketPackage
{
    public byte[] EncodePacket(int packetId)
    {
        var abc = PasswordUtil.RandomX(3);
        var xp = PasswordUtil.RandomX(4, 120);
        var passBuffer = PasswordUtil.GetCode(abc[0], abc[1], abc[2], xp);

        Helper.encryptKey = Encoding.UTF8.GetString(passBuffer);
        if (Stream == null)
            return Array.Empty<byte>();
        var buffer = Stream.GetBytes();

        DataStream.WriteInt32(packetId);

        var encodeBuffer = Gzip.CompressBytes(Helper.EncryptDll(buffer));
        DataStream.WriteInt32(encodeBuffer.Length);
        DataStream.WriteBytes(encodeBuffer);

        DataStream.WriteInt32(passBuffer.Length);
        DataStream.WriteBytes(passBuffer);

        //return Gzip.CompressBytes(DataStream.GetBytes());
        return DataStream.GetBytes();
    }
}