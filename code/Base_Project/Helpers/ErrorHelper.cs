using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace Base_Project.Helpers
{
    public static class ErrorHelper
    {
        /// <summary>
        /// 取得錯誤訊息集合
        /// </summary>
        /// <returns></returns>
        public static List<ErrorMsg> getErrorMsgs(ModelStateDictionary modelState){
            List<ErrorMsg> errorMsgs = new List<ErrorMsg>();

            if (!modelState.IsValid)
            {
                foreach(var error in modelState.Where(x => x.Value.Errors.Count > 0))
                {
                    errorMsgs.Add(new ErrorMsg() { ErrorID = error.Key, ErrorText = error.Value.Errors[0].ErrorMessage });
                }
            }

            return errorMsgs;
        }
    }
    public class ErrorMsg
    {
        /// <summary>
        /// 錯誤ID
        /// </summary>
        public string ErrorID { set; get; }
        /// <summary>
        /// 錯誤提示文字
        /// </summary>
        public string ErrorText { set; get; }
    }
}
