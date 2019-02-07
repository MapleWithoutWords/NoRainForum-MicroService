using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class ValidCodeHelper
    {
        private static char GetOper()
        {
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            char[] str = { '+', '-', '*', '$' };
            return str[rd.Next(0, 4)];
        }
        public static string CreateVerifyCode(out string res)
        {
            int[] data = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            StringBuilder sb = new StringBuilder();
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            int a = data[rd.Next(0, 10)];
            int b = data[rd.Next(0, 10)];
            switch (GetOper())
            {
                case '+':
                    res = (a + b).ToString();
                    sb.Append(a)
                        .Append("加")
                        .Append(b)
                        .Append("等于几");
                    return sb.ToString();
                case '-':
                    if (a < b)
                    {
                        a = a + b;
                        b = a - b;
                        a = a - b;
                    }
                    res = (a - b).ToString();
                    sb.Append(a)
                        .Append("减")
                        .Append(b)
                        .Append("等于几");
                    return sb.ToString();
                case '*':
                    res = (a * b).ToString();
                    sb.Append(a)
                        .Append("乘")
                        .Append(b)
                        .Append("等于几");
                    return sb.ToString();
                case '$':
                default:
                    for (int i = 0; i < 4; i++)
                    {
                        sb.Append(data[rd.Next(0, 10)]);
                    }
                    res = sb.ToString();
                    return res;
            }
        }
        public static string CreateEmailCode()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(1111, 999999).ToString();
        }
    }
}
