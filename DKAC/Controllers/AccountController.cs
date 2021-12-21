using DKAC.Common;
using DKAC.Models;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using DKAC.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DKAC.Controllers
{
    public class AccountController : Controller
    {
        public LoginRepository _loginRepo = new LoginRepository();

        // GET: Account
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var userName = _loginRepo.GetUserByUserName(login.UserName);
                if (userName != null)
                {
                    bool authenSuccess;
                    authenSuccess = Encryption.CheckPassword(login.Password, userName.PassWord, "");
                    if (authenSuccess)
                    {
                        var user = _loginRepo.GetUserByUserName(login.UserName);
                        var pagemodul = _loginRepo.GetAccountModulPageInfo(user.id);
                        Session.Add(CommonConstants.USER_SESSION, user);
                        Session.Add(CommonConstants.PAGE_MODUL_SESSION, pagemodul);

                        //_loginRepo.InsertOnlineUser(user); // add vào bảng những user đang online khi đăng nhập thành công

                        //add những user online vào cache
                        if (HttpRuntime.Cache["OnlineUsers"] != null)
                        {
                            Dictionary<int, DateTime> onlineUsers = (Dictionary<int, DateTime>)HttpRuntime.Cache["OnlineUsers"];
                            var cache = onlineUsers.Where(x => x.Key == user.id).FirstOrDefault();
                            if (cache.Key > 0)
                            {
                                onlineUsers.Remove(cache.Key);
                            }
                            onlineUsers.Add(user.id, DateTime.Now);
                            HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                        }
                        else
                        {
                            Dictionary<int, DateTime> onlineUsers = new Dictionary<int, DateTime>();
                            onlineUsers.Add(user.id, DateTime.Now);
                            HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                        }


                        if (IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);


                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác!");
            }
            return View("Login");
        }

        /// <summary>
        ///     Checking return url is same with the current host or not
        /// </summary>
        /// <param name="url">return url string</param>
        /// <returns></returns>
        public static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            return url[0] == '/' && (url.Length == 1 || url[1] != '/' && url[1] != '\\') || // "/" or "/foo" but not "//" or "/\"
                   url.Length > 1 &&
                   url[0] == '~' && url[1] == '/'; // "~/" or "~/foo"
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(EmployeeInfo model)
        {
            if (ModelState.IsValid)
            {
                bool result = _loginRepo.SignUp(model);
                if (result)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            if (HttpRuntime.Cache["OnlineUsers"] != null)
            {
                Dictionary<int, DateTime> onlineUsers = (Dictionary<int, DateTime>)HttpRuntime.Cache["OnlineUsers"];
                var cache = onlineUsers.Where(x => x.Key == user.id).FirstOrDefault();
                if (cache.Key > 0)
                {
                    onlineUsers.Remove(cache.Key);
                    HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                }
            }

            if (user.UserGroupId != (int)GroupUser.admin)
            {
                if (HttpRuntime.Cache["MessageUsers"] != null)
                {
                    ConcurrentDictionary<int, string> messageUsers = (ConcurrentDictionary<int, string>)HttpRuntime.Cache["MessageUsers"];
                    var cache = messageUsers.Where(x => x.Key == user.id).FirstOrDefault();
                    if (cache.Key > 0)
                    {
                        var value = cache.Value;
                        messageUsers.TryRemove(cache.Key, out value);
                    }
                }
            }
            //_loginRepo.RemoveOffUser(user); //xóa user khỏi bảng user đang online khi đăng xuất

            Session.Remove(CommonConstants.EMPLOYEE_SESSION);
            Session.Remove(CommonConstants.USER_SESSION);
            return RedirectToAction("Login", "Account");
        }
    }
}