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
    public class SysUserController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Sysmgt/SYS_USER
        public ActionResult Index()
        {
            return View(db.SYS_USER.ToList());
        }

        // GET: Sysmgt/SYS_USER/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SYS_USER sYS_USER = db.SYS_USER.Find(id);
            if (sYS_USER == null)
            {
                return HttpNotFound();
            }
            return View(sYS_USER);
        }

        // GET: Sysmgt/SYS_USER/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sysmgt/SYS_USER/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SID,CSID,MSID,CDT,MDT,ENABLED,EMAIL,ACCT,PWD,HASHKEY,NAME")] SYS_USER sYS_USER)
        {
            if (ModelState.IsValid)
            {
                db.SYS_USER.Add(sYS_USER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sYS_USER);
        }

        // GET: Sysmgt/SYS_USER/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SYS_USER sYS_USER = db.SYS_USER.Find(id);
            if (sYS_USER == null)
            {
                return HttpNotFound();
            }
            return View(sYS_USER);
        }

        // POST: Sysmgt/SYS_USER/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SID,CSID,MSID,CDT,MDT,ENABLED,EMAIL,ACCT,PWD,HASHKEY,NAME")] SYS_USER sYS_USER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sYS_USER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sYS_USER);
        }

        // GET: Sysmgt/SYS_USER/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SYS_USER sYS_USER = db.SYS_USER.Find(id);
            if (sYS_USER == null)
            {
                return HttpNotFound();
            }
            return View(sYS_USER);
        }

        // POST: Sysmgt/SYS_USER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SYS_USER sYS_USER = db.SYS_USER.Find(id);
            db.SYS_USER.Remove(sYS_USER);
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
