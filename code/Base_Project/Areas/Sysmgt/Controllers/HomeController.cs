using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Base_Project.Areas.Sysmgt.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Sysmgt/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}