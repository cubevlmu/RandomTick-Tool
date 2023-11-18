namespace ClsOom.ClassOOM.model.Tools
{
    internal sealed class SignSIntUtil
    {
        public static byte[] ParseInt32ToSInt(int source)
        {
            bool negtive = source < 0;
            int a = negtive ? -source : source;
            List<byte> result = new List<byte>();

            do
            {
                var done = a / 255;
                var o = a % 255;

                result.Add((byte)o);
                if (done <= 255)
                {
                    result.Add((byte)done);
                    break;
                }
                else
                    a = done;
            } while (true);
            result.Insert(0, (byte)result.Count);
            result.Insert(0, negtive ? (byte)1 : (byte)0);

            return result.ToArray();
        }

        public static byte[] ParseInt64ToSLong(long source)
        {
            bool negtive = source < 0;
            long a = negtive ? -source : source;
            List<byte> result = new List<byte>();

            do
            {
                var done = a / 255;
                var o = a % 255;

                result.Add((byte)o);
                if (done <= 255)
                {
                    result.Add((byte)done);
                    break;
                }
                else
                    a = done;
            } while (true);
            result.Insert(0, (byte)result.Count);
            result.Insert(0, negtive ? (byte)1 : (byte)0);

            return result.ToArray();
        }

        public static int ReadSInt2Int32(PacketStream stream)
        {
            int baseFor = 0;
            int len = stream.ReadByte();
            bool isNagitive = stream.ReadByte() == 1 ? true : false;

            for(int i = 0; i < len; i++)
            {
                if(i is 0)
                    baseFor = stream.ReadByte();
                baseFor *= stream.ReadByte();
            }

            if (isNagitive)
                return -baseFor;
            return baseFor;
        }

        public static long ReadSLong2Int64(PacketStream stream)
        {
            long baseFor = 0;
            int len = stream.ReadByte();
            bool isNagitive = stream.ReadByte() == 1 ? true : false;

            for (int i = 0; i < len; i++)
            {
                if (i is 0)
                    baseFor = stream.ReadByte();
                baseFor *= stream.ReadByte();
            }

            if (isNagitive)
                return -baseFor;
            return baseFor;
        }
    }
}
