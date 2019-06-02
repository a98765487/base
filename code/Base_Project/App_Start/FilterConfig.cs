using System;
using System.Web;
using System.Web.Mvc;
using Base_Project.Helpers;

namespace Base_Project
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ModelStateValidationFilter());
        }
    }
}
