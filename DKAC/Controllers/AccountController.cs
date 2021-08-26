using DKAC.Common;
using DKAC.Models;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Repository;
using System;
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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
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
                        var em = _loginRepo.GetEmployeeByUserName(login.UserName);
                        var user = _loginRepo.GetUserByUserName(login.UserName);
                        var pagemodul = _loginRepo.GetAccountModulPageInfo(user.id);
                        Session.Add(CommonConstants.EMPLOYEE_SESSION, em);
                        Session.Add(CommonConstants.USER_SESSION, user);
                        Session.Add(CommonConstants.PAGE_MODUL_SESSION, pagemodul);

                        //_loginRepo.InsertOnlineUser(user); // add vào bảng những user đang online khi đăng nhập thành công

                        //add những user online vào cache
                        if (HttpRuntime.Cache["LoggedInUsers"] != null)
                        {
                            Dictionary<int, string> loggedInUsers = (Dictionary<int, string>)HttpRuntime.Cache["LoggedInUsers"];
                            var cache = loggedInUsers.Where(x => x.Key == user.id).FirstOrDefault();
                            if (cache.Key <= 0) //kiểm tra nếu trong cache chưa có thì mới add vào
                            {
                                loggedInUsers.Add(user.id, $"{user.UserName}-{user.FullName}");
                                HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                                if(HttpRuntime.Cache["OnlineUsers"] != null)
                                {
                                    Dictionary<int, DateTime> onlineUsers = (Dictionary<int, DateTime>)HttpRuntime.Cache["OnlineUsers"];
                                    onlineUsers.Add(user.id, DateTime.Now);
                                    HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                                }
                            }
                        }
                        else
                        {
                            Dictionary<int, string> loggedInUsers =new Dictionary<int, string>();
                            Dictionary<int, DateTime> onlineUsers = new Dictionary<int, DateTime>();
                            loggedInUsers.Add(user.id, $"{user.UserName}-{user.FullName}");
                            onlineUsers.Add(user.id, DateTime.Now);
                            HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                            HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác!");
            }
            return View("Login");
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
            if (HttpRuntime.Cache["LoggedInUsers"] != null)
            {
                Dictionary<int, string> loggedInUsers = (Dictionary<int, string>)HttpRuntime.Cache["LoggedInUsers"];
                var cache = loggedInUsers.Where(x => x.Key == user.id).FirstOrDefault();
                if (cache.Key >= 0)
                {
                    loggedInUsers.Remove(cache.Key);
                    HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                    if(HttpRuntime.Cache["OnlineUsers"] != null)
                    {
                        Dictionary<int, DateTime> onlineUsers = (Dictionary<int, DateTime>)HttpRuntime.Cache["OnlineUsers"];
                        onlineUsers.Remove(cache.Key);
                    }
                }
                
                //string userCache = $"ID_{user.id}-";
                //List<string> loggedInUsers = (List<string>)HttpRuntime.Cache["LoggedInUsers"];
                //var cache = loggedInUsers.Where(x => x.Contains(userCache)).FirstOrDefault();

                //if (cache != null)
                //{
                //    loggedInUsers.Remove(cache); //remove user khỏi list user đang online
                //}
            }

            //_loginRepo.RemoveOffUser(user); //xóa user khỏi bảng user đang online khi đăng xuất

            Session.Remove(CommonConstants.EMPLOYEE_SESSION);
            Session.Remove(CommonConstants.USER_SESSION);
            return RedirectToAction("Login", "Account");
        }
    }
}