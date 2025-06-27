using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Core.Helpers
{
    public class MD5Helper
    {
        public static string MD5Encryp(string pwd)
        {
            //获取加密后的密码 MD5 32位加密
            MD5 md5 = MD5.Create();
            byte[] p = md5.ComputeHash(Encoding.Default.GetBytes(pwd));
            string byte2String = string.Empty;
            for (int i = 0; i < p.Length; i++)
            {
                byte2String += p[i].ToString("X2");
            }
            return byte2String;
        }
    }
}
