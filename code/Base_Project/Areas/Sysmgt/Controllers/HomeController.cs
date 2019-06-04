using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Base_Project.Entity;
using Base_Project.Models.Home;
using Base_Project.Helpers;

namespace Base_Project.Areas.Sysmgt.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            if (!SysHelper.init(model))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult Login()
        {
            var model = new LoginViewModel();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(LoginViewModel model)
        {
            List<ErrorMsg> ErrorMsgs = new List<ErrorMsg>();

            if (Session["ValidateCode"] == null)
            {
                ErrorMsgs.Add(new ErrorMsg() { ErrorID = "ValidateCode", ErrorText = "驗證碼已經過期" });
                return Json(new { Success = false, ErrorMsgs });
            }

            if (!(Session["ValidateCode"].ToString().ToLower() == model.ValidateCode.ToLower()))
            {
                ErrorMsgs.Add(new ErrorMsg() { ErrorID = "ValidateCode", ErrorText = "驗證碼輸入錯誤" });
                return Json(new { Success = false, ErrorMsgs });
            }

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    var sysUser = db.SYS_USER.Where(x => x.ACCT == model.Acct).SingleOrDefault();
                    if (sysUser == null)
                    {
                        ErrorMsgs.Add(new ErrorMsg() { ErrorID = "Acct" , ErrorText = "找不到此系統管理者" });
                        return Json(new { Success = false, ErrorMsgs });
                    }
                    else
                    {

                        if (sysUser.PWD == MemberHelper.getHashPwd(model.Pwd + sysUser.HASHKEY))
                        {
                            //登入成功
                            Session["ActorSId"] = sysUser.SID;
                        }
                        else
                        {
                            ErrorMsgs.Add(new ErrorMsg() { ErrorID = "Pwd", ErrorText = "密碼錯誤" });
                            return Json(new { Success = false, ErrorMsgs });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }

            return Json(new { Success = true });
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }
    }
}