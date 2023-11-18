namespace ClsOom.ClassOOM.model.Tools
{
    internal sealed class VarNumberUtil
    {
        public static byte[] ParseInt32ToVarInt(int source)
        {
            var unsigned = (uint)source;
            var result = new List<byte>();
            do
            {
                var temp = (byte)(unsigned & 127);
                unsigned >>= 7;

                if (unsigned != 0)
                    temp |= 128;

                result.Add(temp);
            }
            while (unsigned != 0);
            return result.ToArray();
        }

        public static byte[] ParseInt64ToVarLong(long source)
        {
            var unsigned = (ulong)source;
            List<byte> result = new List<byte>();

            do
            {
                var temp = (byte)(unsigned & 127);

                unsigned >>= 7;

                if (unsigned != 0)
                    temp |= 128;


                result.Add(temp);
            }
            while (unsigned != 0);
            return result.ToArray();
        }

        public static int ReadVarInt2Int32(PacketStream stream)
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = stream.ReadByte();
                int value = read & 0b01111111;
                result |= value << (7 * numRead);

                numRead++;
                if (numRead > 5)
                {
                    throw new InvalidOperationException("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public static long ReadVarLong2Int64(PacketStream stream)
        {
            int numRead = 0;
            long result = 0;
            byte read;
            do
            {
                read = stream.ReadByte();
                int value = (read & 0b01111111);
                result |= (long)value << (7 * numRead);

                numRead++;
                if (numRead > 10)
                {
                    throw new InvalidOperationException("VarLong is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }
    }
}
