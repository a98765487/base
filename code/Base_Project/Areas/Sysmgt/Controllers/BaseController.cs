using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base_Project.Entity;
using NLog;
using System.IO;

namespace Base_Project.Areas.Sysmgt.Controllers
{
    public class BaseController : Controller
    {
        protected static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            string folder = "/Upload/Product/";
            //string fileName = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            string filePath = folder + file.FileName;

            if (!Directory.Exists(Server.MapPath(folder)))
            {
                Directory.CreateDirectory(Server.MapPath(folder));
            }

            file.SaveAs(Server.MapPath(filePath));
            return Json(new { location = filePath });
        }
    }
}