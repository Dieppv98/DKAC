using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using DKAC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Controllers
{
    public class HomeController : BaseController
    {
        HomeRepository _homeRepo = new HomeRepository();

        public ActionResult Index(HomeRequestModel model)
        {
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            List<DishInfo> lstInfo = new List<DishInfo>();
            if (em != null)
            {
                var lstDish = _homeRepo.GetAllDishs(null);
                foreach (var item in lstDish)
                {
                    DishInfo info = new DishInfo();
                    info.id = item.id;
                    info.Image = item.Image;
                    info.JugmentPoint = item.JugmentPoint;
                    info.JugmentQty = item.JugmentQty;
                    info.RegisterQty = item.RegisterQty;
                    info.SupplierId = item.SupplierId;
                    info.DishCode = item.DishCode;
                    info.DishName = item.DishName;
                    info.DishTypeId = item.DishTypeId;
                    info.Cost = item.Cost;
                    info.Description = item.Description;
                    var jug = _homeRepo.GetJug(item.id, em.id);
                    if (jug != null) { info.JugStatus = 1; }
                    else { info.JugStatus = 0; }
                    lstInfo.Add(info);
                }
            }
            model.lstDish = lstInfo;
            var lstDishType = _homeRepo.GetAllDishTypes();
            model.lstDishType = lstDishType;
            return View(model);
        }

        public PartialViewResult ListDish(int? typeId)
        {
            var lstDish = _homeRepo.GetAllDishs(typeId);
            ViewBag.lstDish = lstDish;
            return PartialView();
        }

        public ActionResult NotPermission()
        {
            return View();
        }
    }
}