using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web;

namespace Base_Project.Helpers
{
    public class ModelStateValidationFilter : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var errors = ErrorHelper.getErrorMsgs(filterContext.Controller.ViewData.ModelState);
                var errorMsg = string.Empty;
                foreach(var error in errors)
                {
                    errorMsg += string.Format("<p>{0}</p>", error.ErrorText);
                }
                throw new HttpException(500, errorMsg);
            }
        }
    }
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
