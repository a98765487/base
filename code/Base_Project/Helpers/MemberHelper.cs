using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Base_Project.Helpers
{
    public static class MemberHelper
    {
        /// <summary>
        /// 取得密碼Salt
        /// </summary>
        /// <returns></returns>
        public static string getHashKey(){
            return Guid.NewGuid().ToString("N").Substring(0,10);
        }
        /// <summary>
        /// 取得密碼雜湊值
        /// </summary>
        /// <returns></returns>
        public static string getHashPwd(string pwd)
        {
            // SHA256 加密
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(pwd);
            byte[] crypto = sha256.ComputeHash(source);
            return Convert.ToBase64String(crypto);
        }
    }
}
