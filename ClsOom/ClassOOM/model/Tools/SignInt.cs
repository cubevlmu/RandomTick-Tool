namespace ClsOom.ClassOOM.model.Tools
{
    internal static class SignInt
    {
        public static long Bit10ToInt64(byte[] input)
        {
            long baseFor = 0;

            var negative = input[0] is 1;
            var isFirstNonZero = true;

            for (var i = 1; i < 10; i++)
            {
                if (input[i] != 0 && isFirstNonZero)
                {
                    if (i is 9)
                    {
                        baseFor = input[i];
                        break;
                    }
                    baseFor = input[i] * 255;
                    isFirstNonZero = false;
                }
                else if (input[i] != 0 && i != 9)
                {
                    baseFor += input[i];
                    baseFor *= 255;
                }
                else if (input[i] != 0 && i == 9)
                {
                    baseFor += input[i];
                    break;
                }
            }

            if (negative)
                return -baseFor;
            return baseFor;
        }

        public static byte[] Int64ToBit10(long value)
        {
            var negative = value < 0;

            var a = negative ? -value : value;
            var result = new byte[10];
            for (var i = 0; i < 10; i++)
            {
                var done = a / 255;
                var o = a % 255;

                result[4 - i] = (byte)o;
                if (done <= 255)
                {
                    result[4 - i - 1] = (byte)done;
                    break;
                }

                a = done;
            }
            if (negative)
                result[0] = 1;
            return result;
        }

        
        public static int Bit5ToInt32(byte[] input)
        {
            var baseFor = 0;

            var negative = input[0] is 1;
            var isFirstNonZero = true;

            for (var i = 1; i < 5; i++)
            {
                if (input[i] != 0 && isFirstNonZero)
                {
                    if(i is 4)
                    {
                        baseFor = input[i];
                        break;
                    }
                    baseFor = input[i] * 255;
                    isFirstNonZero = false;
                }
                else if (input[i] != 0 && i != 4)
                {
                    baseFor += input[i];
                    baseFor *= 255;
                }
                else if (input[i] != 0 && i == 4)
                {
                    baseFor += input[i];
                    break;
                }
            }

            if (negative)
                return -baseFor;
            return baseFor;
        }

        
        public static byte[] Int32ToBit5(int value)
        {
            var negative = value < 0;

            var a = negative? -value : value;
            var result = new byte[5];
            for (var i = 0; i < 5; i++)
            {
                var done = a / 255;
                var o = a % 255;

                result[4 - i] = (byte)o;
                if (done <= 255)
                {
                    result[4 - i - 1] = (byte)done;
                    break;
                }

                a = done;
            }
            if (negative)
                result[0] = 1;
            return result;
        }
    }
}
