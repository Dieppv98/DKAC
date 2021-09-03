using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DKAC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["DKACDbContext"].ConnectionString;

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (sender is HttpApplication app)
                app.Context.Response.Headers.Remove("Server");
        }

        /// <summary>
        /// PreSend Request Headers
        /// </summary>
        protected void Application_PreSendRequestHeaders()
        {
            //Remove Server Header
            Response.Headers.Remove("Server");
            //Remove X-AspNet-Version Header
            Response.Headers.Remove("X-AspNet-Version");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //NotificationComponent NC = new NotificationComponent();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["LastUpdated"] = currentTime;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Application.Lock();
            //Application["TotalOnlineUsers"] = (int)Application["TotalOnlineUsers"] - 1;
            //Application.UnLock();
        }
        
    }
}
