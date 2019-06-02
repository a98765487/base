using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Base_Project.Entity;
using EntityState = System.Data.Entity.EntityState;
using Base_Project.Models.SysUser;
using Base_Project.Helpers;

namespace Base_Project.Areas.Sysmgt.Controllers
{
    public class SysUserController : BaseController
    {
        #region Index
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            GetPageList(model);

            return View(model);
        }
        [ValidateAntiForgeryToken]
        public ActionResult GetList(IndexViewModel model)
        {
            GetPageList(model);
            return View("_PageList", model);
        }
        private void GetPageList(IndexViewModel model)
        {
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var sysUsers = db.SYS_USER.Select(x => x);
                    if (!String.IsNullOrEmpty(model.Keyword))
                    {
                        sysUsers = sysUsers.Where(x => x.EMAIL.Contains(model.Keyword)
                        || x.NAME.Contains(model.Keyword)
                        || x.ACCT.Contains(model.Keyword)
                        );
                    }

                    model.TotalCount = sysUsers.Count();
                    model.PageSize = 10;

                    var totalPage = (model.TotalCount + model.PageSize - 1) / model.PageSize;
                    var startRow = model.PageSize * (model.PageIndex - 1);
                    var num = startRow + 1;

                    sysUsers = sysUsers.OrderByDescending(x => x.CDT).Skip(startRow).Take(model.PageSize);

                    foreach (var item in sysUsers.ToList())
                    {
                        model.Items.Add(new PageListItem()
                        {
                            Num = num++.ToString(),
                            SId = item.SID,
                            Acct = item.ACCT,
                            Mdt = item.MDT.ToString("yyyy年MM月dd日 HH:mm:ss"),
                            Name = item.NAME,
                        });
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    throw ex;
                }
            }
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            var model = new EditViewModel();
            if (String.IsNullOrEmpty(id))
            {
                return View(model);
            }
            else
            {
                using (DBEntities db = new DBEntities())
                {
                    try
                    {
                        SYS_USER sysUser = db.SYS_USER.Find(id);
                        if (sysUser == null)
                        {
                            return HttpNotFound();
                        }

                        //帶入資料
                        model.CDT = sysUser.CDT;
                        model.CSID = sysUser.CSID;
                        model.EMAIL = sysUser.EMAIL;
                        model.ACCT = sysUser.ACCT;
                        model.ENABLED = sysUser.ENABLED;
                        model.MDT = sysUser.MDT;
                        model.MSID = sysUser.MSID;
                        model.NAME = sysUser.NAME;
                        model.SID = sysUser.SID;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        throw ex;
                    }
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EditViewModel model)
        {
            List<ErrorMsg> ErrorMsgs = new List<ErrorMsg>();
            //新增
            if (String.IsNullOrEmpty(model.SID))
            {
                //新增要輸入密碼
                if (String.IsNullOrEmpty(model.PWD))
                {
                    ErrorMsgs.Add(new ErrorMsg() { ErrorID = "PWD", ErrorText = "請輸入密碼" });
                }

                using (DBEntities db = new DBEntities())
                {
                    var existUser = db.SYS_USER.AsNoTracking().Where(x => x.ACCT == model.ACCT).SingleOrDefault();
                    if (existUser != null) {
                        ErrorMsgs.Add(new ErrorMsg() { ErrorID = "ACCT", ErrorText = "使用者帳號已經使用" });
                    }
                }

                model.SID = SystemIdHelper.getNewSId();

                SYS_USER sysUser = new SYS_USER()
                {
                    SID = model.SID,
                    CDT = DateTime.Now,
                    MDT = DateTime.Now,
                    CSID = SystemIdHelper.getEmptySId(),
                    MSID = SystemIdHelper.getEmptySId(),
                    ENABLED = model.ENABLED,
                    EMAIL = model.EMAIL,
                    NAME = model.NAME,
                    ACCT = model.ACCT,
                    HASHKEY = MemberHelper.getHashKey(),
                    PWD = MemberHelper.getHashPwd(model.PWD + model.HASHKEY),
                };

                if (ErrorMsgs.Count > 0)
                {
                    return Json(new { Success = false, ErrorMsgs });
                }
                using (DBEntities db = new DBEntities())
                {
                    try
                    {
                        db.SYS_USER.Add(sysUser);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ErrorMsgs.Add(new ErrorMsg() { ErrorID = "Utility", ErrorText = ex.Message });
                        return Json(new { Success = false, ErrorMsgs });
                    }
                }
            }
            //修改
            else
            {
                //查詢既有資料
                using (DBEntities db = new DBEntities())
                {
                    var oldInfo = db.SYS_USER.AsNoTracking().Where(x => x.SID == model.SID).Single();
                    SYS_USER newInfo = new SYS_USER()
                    {
                        SID = model.SID,
                        CDT = oldInfo.CDT,
                        MDT = DateTime.Now,
                        CSID = oldInfo.CSID,
                        MSID = SystemIdHelper.getEmptySId(),
                        ENABLED = model.ENABLED,
                        EMAIL = model.EMAIL,
                        NAME = model.NAME,
                        HASHKEY = oldInfo.HASHKEY,
                        PWD = oldInfo.PWD,
                        ACCT = oldInfo.ACCT,
                    };
                    if (!String.IsNullOrEmpty(model.PWD))
                    {
                        newInfo.HASHKEY = MemberHelper.getHashKey();
                        newInfo.PWD = MemberHelper.getHashPwd(model.PWD + model.HASHKEY);
                    }

                    if (ErrorMsgs.Count > 0)
                    {
                        return Json(new { Success = false, ErrorMsgs });
                    }
                    db.Entry(newInfo).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(new { Success = true });
        }
        #endregion

        #region Delete
        public JsonResult Delete(FormCollection form)
        {
            List<ErrorMsg> ErrorMsgs = new List<ErrorMsg>();

            if (String.IsNullOrEmpty(form["selectItems"]))
            {
                ErrorMsgs.Add(new ErrorMsg() { ErrorID = "Utility", ErrorText = "請選擇至少一個項目" });
                return Json(new { Success = false, ErrorMsgs });
            }

            foreach (var sid in form["selectItems"].Split(','))
            {
                if (!SystemIdHelper.checkSId(sid))
                {
                    ErrorMsgs.Add(new ErrorMsg() { ErrorID = "Utility", ErrorText = "包含不合法的系統代號" });
                    return Json(new { Success = false, ErrorMsgs });
                }
            }
            foreach (var sid in form["selectItems"].Split(','))
            {
                using (DBEntities db = new DBEntities())
                {
                    try
                    {
                        var sysUser = db.SYS_USER.Single(x => x.SID == sid);
                        db.SYS_USER.Remove(sysUser);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ErrorMsgs.Add(new ErrorMsg() { ErrorID = "Utility", ErrorText = ex.Message });
                        return Json(new { Success = false, ErrorMsgs });
                    }
                }
            }
            return Json(new { Success = true });
        }
        #endregion
    }
}
