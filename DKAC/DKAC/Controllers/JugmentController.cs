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
    public class JugmentController : Controller
    {
        JugmentRepository _jugRepo = new JugmentRepository();

        // GET: Jugment
        public ActionResult Index(int? dishId)
        {
            var dish = _jugRepo.GetDish(dishId);
            ViewBag.Dish = dish;
            return View();
        }

        public ActionResult Jugment(Jugment model)
        {
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            model.EmployeeId = em.id;
            model.JugmentDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            var rs = _jugRepo.Jugment(model);
            if (rs == 1)
            {
                return Json(new { status = 1, message = "Cảm ơn bạn đã đánh giá" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Đánh giá thất bại" }, JsonRequestBehavior.AllowGet);
        }
    }
}