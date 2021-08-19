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
            Session.Remove(CommonConstants.EMPLOYEE_SESSION);
            Session.Remove(CommonConstants.USER_SESSION);
            return RedirectToAction("Login", "Account");
        }
    }
}