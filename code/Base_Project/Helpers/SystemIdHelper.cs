using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Project.Helpers
{
    public static class SystemIdHelper
    {
        /// <summary>
        /// 取得新的系統代號
        /// </summary>
        /// <returns></returns>
        public static string getNewSId(){
            var sid = String.Empty;
            sid = "0" + Guid.NewGuid().ToString("N").Substring(0,19).ToUpper();
            return sid;
        }
        /// <summary>
        /// 取得空的系統代號
        /// </summary>
        /// <returns></returns>
        public static string getEmptySId()
        {
            var sid = String.Empty;
            sid = "00000000000000000001";
            return sid;
        }
        /// <summary>
        /// 檢查系統代號是否合法
        /// </summary>
        /// <returns></returns>
        public static bool checkSId(string sid)
        {
            return sid.Length == 20 && sid.Substring(0,1) == "0";
        }
    }
}
