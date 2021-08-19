using DKAC.Common;
using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Controllers
{

    public class ChartController : BaseController
    {
        ChartRepository _chartRepo = new ChartRepository();

        // GET: Chart
        public ActionResult Index()
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.ThongKeLuotSuDungHeThong, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            return View();
        }

        [HttpGet]
        public JsonResult LoadChart()
        {
            var rs = _chartRepo.LoadChart();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}