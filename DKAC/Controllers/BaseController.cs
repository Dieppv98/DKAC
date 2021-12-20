using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Repository;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using System.Text;
using LogLevel = NLog.LogLevel;
using NLog.Targets;
using FlexCel.Core;
using FlexCel.Render;
using FlexCel.Pdf;
using System.IO;
using OfficeOpenXml;
using FlexCel.XlsAdapter;
using System.Web.Security;

namespace DKAC.Controllers
{
    public class BaseController : Controller
    {
        DKACDbContext db = new DKACDbContext();
        public LoginRepository _loginRepo = new LoginRepository();
        AccountController _acc = new AccountController();
        private User _currentUser;
        public Logger Log;

        public BaseController()
        {
            LogConfig();
            Log = LogManager.GetLogger(this.GetType().Name);
        }

        private static void LogConfig()
        {
            try
            {
                var config = new LoggingConfiguration();
                var fileTarget = new FileTarget
                {
                    FileName =
                        "${basedir}/_WebLog/${shortdate}/${logger}.log",
                    //Layout =
                    //    "${date:format=dd/MM/yyyy HH\\:mm\\:ss\\.fff}|${threadid}|${level}|${logger}|${message}",
                    Layout =
                        "${date:format=dd/MM/yyyy HH\\:mm\\:ss\\.fff}|${threadid}|${level}|${message}",
                    ArchiveAboveSize = 5242880,
                    Encoding = Encoding.UTF8
                };
                config.AddTarget("file", fileTarget);
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, fileTarget));
                LogManager.Configuration = config;
            }
            catch (Exception)
            {
            }
        }

        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext fillterContext)
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            if (user == null)
            {
                FormsAuthentication.SignOut();
                string redirectTo = "~/Account/Login";
                if (!string.IsNullOrEmpty(fillterContext.HttpContext.Request.RawUrl))
                {
                    redirectTo = string.Format("~/Account/Login?ReturnUrl={0}", HttpUtility.UrlEncode(fillterContext.HttpContext.Request.Url.PathAndQuery));
                    fillterContext.Result = new RedirectResult(redirectTo);
                    return;
                }
                //fillterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }

            base.OnActionExecuting(fillterContext);
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Error(filterContext.Exception.Message);
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
            filterContext.ExceptionHandled = true;
        }

        public bool CheckPermission(int pageId, int action, User currentUser)
        {
            var lstRole = db.UserRoles.Where(x => x.UserId == currentUser.id).ToList();
            List<PermissionAction> lstPer = new List<PermissionAction>();
            foreach (var item in lstRole)
            {
                var lstPerAction = db.PermissionActions.Where(x => x.RoleId == item.RoleId).ToList();
                if (lstPerAction != null) lstPer.AddRange(lstPerAction);
            }
            lstPer.Distinct().ToList();
            if (currentUser.UserName == "admin") return true;
            int actionkey = 7;
            int action1 = 4;
            var r = actionkey & (byte)action1;
            var check = lstPer.Any(x => x.PageId == pageId && (x.ActionKey & (byte)action) == (byte)action);
            return check;
        }

        public User CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                {
                    _currentUser = null;
                    return _currentUser;
                }

                if (_currentUser != null)
                    return _currentUser;

                var identity = User.Identity as ClaimsIdentity;

                var userName = identity.FindFirstValue("UserName");
                var code = identity.FindFirstValue("Code");

                _currentUser = new User
                {
                    Code = code,
                    UserName = userName
                };
                _currentUser = _loginRepo.GetUserByUserNameByCode(userName, code);
                return _currentUser;
            }
            set => _currentUser = value;
        }

        /// <summary>
        /// Hàm xóa cache
        /// </summary>
        /// <param name="Remove Cache"></param>
        public void RemoveCache(string cache)
        {
            var lstCaches = System.Runtime.Caching.MemoryCache.Default.Where(x => x.Key.ToLower().Contains(cache.ToLower())).ToList();
            for (int i = 0; i < lstCaches.Count; i++)
                System.Runtime.Caching.MemoryCache.Default.Remove(lstCaches[i].Key);
        }


        protected ActionResult ViewReport(ExcelFile xls, string fileName, bool? exportExcel)
        {
            try
            {
                if (exportExcel != null && exportExcel == true)
                {
                    if (xls == null)
                        return new EmptyResult();

                    using (var ms = new MemoryStream())
                    {
                        xls.Save(ms);
                        ms.Position = 0;
                        return File(ms.ToArray(), "application/vnd.xlsx", fileName + ".xlsx");
                    }
                }

                if (xls == null)
                    return new EmptyResult();

                using (var pdf = new FlexCelPdfExport())
                {
                    pdf.Workbook = xls;
                    pdf.FontEmbed = TFontEmbed.Embed;
                    pdf.FontMapping = TFontMapping.ReplaceAllFonts;
                    using (var ms = new MemoryStream())
                    {
                        pdf.BeginExport(ms);
                        pdf.ExportAllVisibleSheets(false, "BaoCao");
                        pdf.EndExport();
                        ms.Position = 0;
                        return File(ms.ToArray(), "application/pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                return new EmptyResult();
            }
        }

        protected ActionResult ExportPDF(ExcelFile xls, string fileName, bool? exportExcel)
        {
            try
            {
                if (xls == null)
                    return new EmptyResult();

                using (var pdf = new FlexCelPdfExport())
                {
                    pdf.Workbook = xls;
                    pdf.FontEmbed = TFontEmbed.Embed;
                    pdf.FontMapping = TFontMapping.ReplaceAllFonts;
                    using (var ms = new MemoryStream())
                    {
                        pdf.BeginExport(ms);
                        pdf.ExportAllVisibleSheets(false, "BaoCao");
                        pdf.EndExport();
                        ms.Position = 0;
                        return File(ms.ToArray(), "application/pdf", fileName + ".pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                return new EmptyResult();
            }
        }

        protected XlsFile CreateXlsFile(ExcelPackage excel)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var result = new XlsFile(true);
            try
            {
                using (var memoryStream = new MemoryStream(excel.GetAsByteArray()))
                {
                    result.Open(memoryStream);
                }
            }
            finally
            {
                ReleaseObjectReport(excel.Workbook);
                excel.Dispose();
            }
            return result;
        }

        protected void ReleaseObjectReport(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}