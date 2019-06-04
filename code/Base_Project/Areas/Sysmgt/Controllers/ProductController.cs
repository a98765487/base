using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Base_Project.Entity;
using Base_Project.Models.Product;
using Base_Project.Helpers;
using Microsoft.Security.Application;

namespace Base_Project.Areas.Sysmgt.Controllers
{
    public class ProductController : BaseController
    {
        #region Index
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            if (!SysHelper.init(model))
            {
                return RedirectToAction("Login", "Home");
            }

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
                    var products = db.PRODUCTs.Select(x => x);
                    if (!String.IsNullOrEmpty(model.Keyword))
                    {
                        products = products.Where(x => x.NAME.Contains(model.Keyword));
                    }

                    model.TotalCount = products.Count();
                    model.PageSize = 10;

                    var totalPage = (model.TotalCount + model.PageSize - 1) / model.PageSize;
                    var startRow = model.PageSize * (model.PageIndex - 1);
                    var num = startRow + 1;

                    products = products.OrderByDescending(x => x.CDT).Skip(startRow).Take(model.PageSize);

                    foreach (var item in products.ToList())
                    {
                        model.Items.Add(new PageListItem()
                        {
                            Num = num++.ToString(),
                            SId = item.SID,
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

            if (!SysHelper.init(model))
            {
                return RedirectToAction("Login", "Home");
            }

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
                        PRODUCT product = db.PRODUCTs.Find(id);
                        if (product == null)
                        {
                            return HttpNotFound();
                        }

                        //帶入資料
                        model.CDT = product.CDT;
                        model.CSID = product.CSID;
                        model.ENABLED = product.ENABLED;
                        model.MDT = product.MDT;
                        model.MSID = product.MSID;
                        model.NAME = product.NAME;
                        model.SID = product.SID;
                        model.CAT_SID = product.CAT_SID;
                        model.CONTENT = Server.HtmlDecode(product.CONTENT);
                        model.DESC = product.DESC;
                        model.IMG_SRC = product.IMG_SRC;
                        model.PRICE = product.PRICE;
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

                model.SID = SystemIdHelper.getNewSId();

                PRODUCT product = new PRODUCT()
                {
                    SID = model.SID,
                    CDT = DateTime.Now,
                    MDT = DateTime.Now,
                    CSID = SystemIdHelper.getEmptySId(),
                    MSID = SystemIdHelper.getEmptySId(),
                    ENABLED = model.ENABLED,
                    NAME = model.NAME,
                    CAT_SID = model.CAT_SID,
                    CONTENT = Encoder.HtmlEncode(model.CONTENT),
                    DESC = model.DESC,
                    IMG_SRC = model.IMG_SRC,
                    PRICE = model.PRICE,
                };

                if (ErrorMsgs.Count > 0)
                {
                    return Json(new { Success = false, ErrorMsgs });
                }
                using (DBEntities db = new DBEntities())
                {
                    try
                    {
                        db.PRODUCTs.Add(product);
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
                    var oldInfo = db.PRODUCTs.AsNoTracking().Where(x => x.SID == model.SID).Single();
                    PRODUCT newInfo = new PRODUCT()
                    {
                        SID = model.SID,
                        CDT = oldInfo.CDT,
                        MDT = DateTime.Now,
                        CSID = oldInfo.CSID,
                        MSID = SystemIdHelper.getEmptySId(),
                        ENABLED = model.ENABLED,
                        NAME = model.NAME,
                        CAT_SID = model.CAT_SID,
                        CONTENT = Encoder.HtmlEncode(model.CONTENT),
                        DESC = model.DESC,
                        IMG_SRC = model.IMG_SRC,
                        PRICE = model.PRICE,
                    };

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
            var model = new IndexViewModel();
            List<ErrorMsg> ErrorMsgs = new List<ErrorMsg>();
            if (!SysHelper.init(model))
            {
                ErrorMsgs.Add(new ErrorMsg() { ErrorID = "Utility", ErrorText = "使用者尚未登入" });
                return Json(new { Success = false, ErrorMsgs });
            }

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
                        var product = db.PRODUCTs.Single(x => x.SID == sid);
                        db.PRODUCTs.Remove(product);
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
