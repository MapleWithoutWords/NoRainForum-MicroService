using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NoRainSDK.hash
{
    internal class MD5Helper
    {
        public static string CalcMD5(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return CalcMD5(bytes);
        }

        public static string CalcMD5(byte[] bytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(bytes);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("x").Length == 1 ? "0" + computeBytes[i].ToString("x") : computeBytes[i].ToString("x");
                }
                return result;
            }
        }

        public static string CalcMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                stream.Position = 0;
                byte[] computeBytes = md5.ComputeHash(stream);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("x").Length == 1 ? "0" + computeBytes[i].ToString("x") : computeBytes[i].ToString("x");
                }
                return result;
            }
        }

        public static string GetSalt(int saltLen)
        {
            StringBuilder sb = new StringBuilder();
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            int len = rd.Next(6, saltLen);
            char[] ch = { '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '{', '}', '|', '?', ':', '<', '>', '？', '·', '。', '！', '‘', '’', '“', '”', '：' };
            for (int i = 0; i < len; i++)
            {
                sb.Append(ch[rd.Next(ch.Length)]);
            }
            return sb.ToString();
        }


    }
}
