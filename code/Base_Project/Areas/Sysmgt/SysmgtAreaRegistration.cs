using System.Web.Mvc;

namespace Base_Project.Areas.Sysmgt
{
    public class SysmgtAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Sysmgt";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Sysmgt_default",
                url: "Sysmgt/{controller}/{action}/{id}",
                defaults :new { controller = "Home",  action = "Index", id = UrlParameter.Optional },
                namespaces : new [] { "Base_Project.Areas.Sysmgt.Controllers" }
            );
        }
    }
}