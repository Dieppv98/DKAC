using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
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
            ViewBag.hasViewPermission = CheckPermission((int)PageId.QuanLyThucDon, (int)Actions.Xem, currentUser);
            ViewBag.hasAddPermission = CheckPermission((int)PageId.QuanLyThucDon, (int)Actions.Them, currentUser);
            ViewBag.hasUpdatePermission = CheckPermission((int)PageId.QuanLyThucDon, (int)Actions.Sua, currentUser);
            ViewBag.hasDeletePermission = CheckPermission((int)PageId.QuanLyThucDon, (int)Actions.Xoa, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var model = _menuRepo.GetAllMenu(KeySearch, page, pageSize);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            MenuInfo model = new MenuInfo();
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            var hasPermission = CheckPermission((int)PageId.QuanLyThucDon, (int)Actions.Them, currentUser);
            if (!hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var allDish = _menuRepo.GetAllDish();
            List<SelectListItem> lstAllDish = allDish.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.DishName.ToString(),
                    Value = a.id.ToString(),
                };
            });
            model.lstDish = lstAllDish;
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckDuplicatedMenuCode(string MenuCode, int? id)
        {
            var name = _menuRepo.GetByMenuCode(MenuCode, id);
            if (name == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

    }
}