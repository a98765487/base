﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Base_Project.Entity;
using EntityState = System.Data.Entity.EntityState;
using Base_Project.Models.Member;
using Base_Project.Helpers;

namespace Base_Project.Areas.Sysmgt.Controllers
{
    public class MemberController : BaseController
    {
        #region Index
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            GetPageList(model);

            return View(model);
        }
        public ActionResult GetList(IndexViewModel model)
        {
            GetPageList(model);
            return View("_PageList",model);
        }
        private void GetPageList(IndexViewModel model)
        {
            using (DBEntities db = new DBEntities())
            {
                var members = db.MEMBERs.Select(x => x);
                if (!String.IsNullOrEmpty(model.Keyword))
                {
                    members = members.Where(x => x.EMAIL.Contains(model.Keyword)
                    || x.NAME.Contains(model.Keyword)
                    );
                }

                model.TotalCount = members.Count();
                model.PageSize = 10;

                var totalPage = (model.TotalCount + model.PageSize - 1) / model.PageSize;
                var startRow = model.PageSize * (model.PageIndex - 1);
                var num = startRow + 1;

                members = members.OrderByDescending(x => x.CDT).Skip(startRow).Take(model.PageSize);

                foreach (var item in members.ToList())
                {
                    model.Items.Add(new Item()
                    {
                        Num = num++.ToString(),
                        SId = item.SID,
                        Email = item.EMAIL,
                        Mdt = item.MDT.ToString("yyyy年MM月dd日 HH:mm:ss"),
                        Name = item.NAME,
                    });
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
                    MEMBER mEMBER = db.MEMBERs.Find(id);
                    if (mEMBER == null)
                    {
                        return HttpNotFound();
                    }

                    //帶入資料
                    model.CDT = mEMBER.CDT;
                    model.CSID = mEMBER.CSID;
                    model.EMAIL = mEMBER.EMAIL;
                    model.ENABLED = mEMBER.ENABLED;
                    model.FBID = mEMBER.FBID;
                    model.GOOGLEID = mEMBER.GOOGLEID;
                    model.MDT = mEMBER.MDT;
                    model.MSID = mEMBER.MSID;
                    model.NAME = mEMBER.NAME;
                    model.SID = mEMBER.SID;
                    model.VERIFY = mEMBER.VERIFY;
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EditViewModel model)
        {
            List<ErrorMsg> ErrorMsgs = new List<ErrorMsg>();
            if (!ModelState.IsValid)
            {
                ErrorMsgs.AddRange(ErrorHelper.getErrorMsgs(ModelState));
            }
            //新增
            if (String.IsNullOrEmpty(model.SID))
            {
                //新增要輸入密碼
                if (String.IsNullOrEmpty(model.PWD))
                {
                    ErrorMsgs.Add(new ErrorMsg() { ErrorID = "PWD" , ErrorText = "請輸入密碼" });
                }

                model.SID = SystemIdHelper.getNewSId();

                MEMBER member = new MEMBER()
                {
                    SID = model.SID,
                    CDT = DateTime.Now,
                    MDT = DateTime.Now,
                    CSID = SystemIdHelper.getEmptySId(),
                    MSID = SystemIdHelper.getEmptySId(),
                    ENABLED = model.ENABLED,
                    EMAIL = model.EMAIL,
                    NAME = model.NAME,
                    FBID = model.FBID,
                    GOOGLEID = model.GOOGLEID,
                    HASHKEY = MemberHelper.getHashKey(),
                    PWD = MemberHelper.getHashPwd(model.PWD + model.HASHKEY),
                    VERIFY = model.VERIFY,
                };

                if (ErrorMsgs.Count > 0)
                {
                    return Json(new { Success = false , ErrorMsgs });
                }
                using (DBEntities db = new DBEntities())
                {
                    db.MEMBERs.Add(member);
                    db.SaveChanges();
                }
            }
            //修改
            else
            {
                //查詢既有資料
                using (DBEntities db = new DBEntities())
                {
                    var oldInfo = db.MEMBERs.AsNoTracking().Where(x => x.SID == model.SID).Single();
                    MEMBER newInfo = new MEMBER()
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
                        FBID = oldInfo.FBID,
                        GOOGLEID = oldInfo.GOOGLEID,
                        VERIFY = model.VERIFY,
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
                        var member = db.MEMBERs.Single(x => x.SID == sid);
                        db.MEMBERs.Remove(member);
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
