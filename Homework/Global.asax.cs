using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Homework.Business.Initialization;
using Homework.Business.Models;
using Homework.DependencyInjection;

namespace Homework
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            DependencyInjectionConfig.RegisterContainer();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
