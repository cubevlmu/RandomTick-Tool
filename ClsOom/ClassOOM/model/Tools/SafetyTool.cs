using System.Security.Cryptography;
using System.Text;

namespace ClsOom.ClassOOM.model.Tools
{
    /// <summary>
    /// 已被弃用，请换用AES算法加密
    /// </summary>
    [Obsolete("Use AES Instead")]
    public sealed class SafetyTool
    {
        public const string EncryptKey = "(B&@";    //定义密钥 

        #region 加密字符串 
        public static string Encrypt(string str)
        {
            var desc = new DESCryptoServiceProvider();   //实例化加/解密类对象  
            var key = Encoding.Unicode.GetBytes(EncryptKey); //定义字节数组，用来存储密钥   
            var data = Encoding.Unicode.GetBytes(str);//定义字节数组，用来存储要加密的字符串 
            using var mStream = new MemoryStream(); //实例化内存流对象     
            using var cStream = new CryptoStream(mStream, desc.CreateEncryptor(key, key), CryptoStreamMode.Write);
            cStream.Write(data, 0, data.Length);  //向加密流中写入数据     
            cStream.FlushFinalBlock();              //释放加密流     
            return Convert.ToBase64String(mStream.ToArray());
        }

        #endregion

        #region 解密字符串  

        public static string Decrypt(string str)
        {
            var desc = new DESCryptoServiceProvider();   //实例化加/解密类对象   
            var key = Encoding.Unicode.GetBytes(EncryptKey); //定义字节数组，用来存储密钥   
            var data = Convert.FromBase64String(str);//定义字节数组，用来存储要解密的字符串 
            using var mStream = new MemoryStream(); //实例化内存流对象     
            using var cStream = new CryptoStream(mStream, desc.CreateDecryptor(key, key), CryptoStreamMode.Write);
            cStream.Write(data, 0, data.Length);      //向解密流中写入数据    
            cStream.FlushFinalBlock();               //释放解密流     
            return Encoding.Unicode.GetString(mStream.ToArray());       //返回解密后的字符串 
        }

        #endregion
    }
}
