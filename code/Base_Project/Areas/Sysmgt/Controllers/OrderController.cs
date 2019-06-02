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
using Base_Project.Models;

namespace Base_Project.Areas.Sysmgt.Controllers
{
    public class OrderController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: ORDER
        public ActionResult Index()
        {
            var oRDERs = db.ORDERs.Include(o => o.MEMBER).Include(o => o.PRODUCT);
            return View(oRDERs.ToList());
        }

        // GET: ORDER/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            return View(oRDER);
        }

        // GET: ORDER/Create
        public ActionResult Create()
        {
            ViewBag.MEMBER_SID = new SelectList(db.MEMBERs, "SID", "CSID");
            ViewBag.PRODUCT_SID = new SelectList(db.PRODUCTs, "SID", "CSID");
            return View();
        }

        // POST: ORDER/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SID,MEMBER_SID,PRODUCT_SID,CSID,MSID,CDT,MDT,COUNT")] ORDER oRDER)
        {
            if (ModelState.IsValid)
            {
                db.ORDERs.Add(oRDER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MEMBER_SID = new SelectList(db.MEMBERs, "SID", "CSID", oRDER.MEMBER_SID);
            ViewBag.PRODUCT_SID = new SelectList(db.PRODUCTs, "SID", "CSID", oRDER.PRODUCT_SID);
            return View(oRDER);
        }

        // GET: ORDER/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            ViewBag.MEMBER_SID = new SelectList(db.MEMBERs, "SID", "CSID", oRDER.MEMBER_SID);
            ViewBag.PRODUCT_SID = new SelectList(db.PRODUCTs, "SID", "CSID", oRDER.PRODUCT_SID);
            return View(oRDER);
        }

        // POST: ORDER/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SID,MEMBER_SID,PRODUCT_SID,CSID,MSID,CDT,MDT,COUNT")] ORDER oRDER)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(oRDER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MEMBER_SID = new SelectList(db.MEMBERs, "SID", "CSID", oRDER.MEMBER_SID);
            ViewBag.PRODUCT_SID = new SelectList(db.PRODUCTs, "SID", "CSID", oRDER.PRODUCT_SID);
            return View(oRDER);
        }

        // GET: ORDER/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            return View(oRDER);
        }

        // POST: ORDER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ORDER oRDER = db.ORDERs.Find(id);
            db.ORDERs.Remove(oRDER);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
