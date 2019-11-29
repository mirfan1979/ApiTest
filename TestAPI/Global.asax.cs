using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TestAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }

        void Application_BeginRequest(Object source, EventArgs e)
        {
            if (string.IsNullOrEmpty(TestAPI.Models.Utility.Constants.BaseUrl))
            {
                var app = (HttpApplication)source;
                var uriObject = app.Context.Request.Url;
                TestAPI.Models.Utility.Constants.BaseUrl = app.Context.Request.Url.OriginalString;
            }
        }
    }
}
