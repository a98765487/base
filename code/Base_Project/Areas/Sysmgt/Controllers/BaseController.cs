using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base_Project.Entity;
using NLog;

namespace Base_Project.Areas.Sysmgt.Controllers
{
    public class BaseController : Controller
    {
        protected static Logger logger = NLog.LogManager.GetCurrentClassLogger();
    }
}