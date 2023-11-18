using System.Text;

namespace ClsOom.ClassOOM.model.Tools
{
    internal abstract class PasswordUtil
    {
        public static int[] RandomX(int times)
        {
            var rd = new Random();
            var ret = new List<int>();
            for(var i = 1; i <= times; i++) ret.Add(rd.Next(1, 100000));
            return ret.ToArray();
        }
        

        public static int[] RandomX(int times,int max)
        {
            var rd = new Random();
            var ret = new List<int>();
            for (var i = 1; i <= times; i++) ret.Add(rd.Next(1, max));
            return ret.ToArray();
        }

        
        public static byte[] GetCode(int a, int b, int c, int[] xp)
        {
            var codeVerify = new List<byte>();
            var xcode = xp.Select(x => (byte)(a * x * x + b * x + c)).ToList();
            foreach (var pc in xcode)
            {
                var toCheck = pc;
                while (true)
                {
                    switch (toCheck)
                    {
                        case < 32:
                            toCheck += 32;
                            break;
                        case > 128:
                            toCheck -= 128;
                            break;
                    }

                    if (toCheck is < 32 or > 128) continue;
                    codeVerify.Add(toCheck); break;
                }
            }
            xcode.Clear();
            return codeVerify.ToArray();
        }

        
        public static byte[] ToUnicodeByte(byte[] input)
        {
            return Encoding.Unicode.GetBytes(Encoding.UTF8.GetString(input));
        }

        public static string ToUtf8String(byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }
    }


    internal class ByteUtil
    {
        /// <summary>  
        /// byte数组转int数组  
        /// </summary>  
        /// <param name="src">源byte数组</param>  
        /// <param name="offset">起始位置</param>  
        /// <returns></returns>  
        public static int[] BytesToInt(byte[] src, int offset)
        {
            var values = new int[src.Length / 4];
            for (var i = 0; i < src.Length / 4; i++)
            {
                var value = (src[offset] & 0xFF)
                            | ((src[offset + 1] & 0xFF) << 8)
                            | ((src[offset + 2] & 0xFF) << 16)
                            | ((src[offset + 3] & 0xFF) << 24);
                values[i] = value;

                offset += 4;
            }
            return values;
        }

        
        public static byte[] intToBytes(int[] src, int offset)
        {
            var values = new byte[src.Length * 4];
            foreach (var t in src)
            {
                values[offset + 3] = (byte)((t >> 24) & 0xFF);
                values[offset + 2] = (byte)((t >> 16) & 0xFF);
                values[offset + 1] = (byte)((t >> 8) & 0xFF);
                values[offset] = (byte)(t & 0xFF);
                offset += 4;
            }
            return values;
        }

        
        public static byte[] CopyTo(byte[] input, byte[] input2)
        {
            var data = new byte[input.Length + input2.Length];
            input.CopyTo(data, 0);
            input2.CopyTo(data, input.Length);
            return data;
        }
        

        public static void CopyTo(byte[] input,byte[] output,int index,int length)
        {
            if (length.Equals(-1)) length = input.Length - 1;
            Array.Copy(input, index, output, output.Length - 1, length);
        }

        
        public static int[] CopyTo(int[] input, int[] input2)
        {
            var data = new int[input.Length + input2.Length];
            input.CopyTo(data, 0);
            input2.CopyTo(data, input.Length);
            return data;
        }
    }
}
