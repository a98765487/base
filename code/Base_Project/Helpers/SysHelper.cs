using Base_Project.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Project.Helpers
{
    public static class SysHelper
    {
        /// <summary>
        /// 系統初始化設定
        /// </summary>
        /// <returns></returns>
        public static bool init(BaseViewModel model)
        {
            //檢查系統使用者是否登入
            if (HttpContext.Current.Session["ActorSId"] != null)
            {
                model.ActorSId = HttpContext.Current.Session["ActorSId"].ToString();
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
