using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Controllers
{
    public class MenuController : BaseController
    {
        MenuRepository _menuRepo = new MenuRepository();

        // GET: Menu
        public ActionResult Index(string KeySearch, int page = 1, int pageSize = 10)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission(4, 1, currentUser);
            ViewBag.hasAddPermission = CheckPermission(4, 2, currentUser);
            ViewBag.hasUpdatePermission = CheckPermission(4, 4, currentUser);
            ViewBag.hasDeletePermission = CheckPermission(4, 8, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var model = _menuRepo.GetAllMenu(KeySearch, page, pageSize);
            return View(model);
        }
    }
}