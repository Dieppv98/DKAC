using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CachingRuntime.Controllers
{
    public class BaseController : Controller
    {
       

        public string AppID = ConfigurationSettings.AppSettings["AppID"];

       
    }
}