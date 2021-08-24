using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using DKAC.Repository;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Controllers
{
    public class HomeController : BaseController
    {
        private IMemoryCache _cache;

        HomeRepository _homeRepo = new HomeRepository();
        NotificationComponent _nc = new NotificationComponent();

        public ActionResult Index(HomeRequestModel model)
        {
            var em = (User)Session[CommonConstants.USER_SESSION];
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

        public ActionResult ListAllDish(HomeRequestModel model, int page = 1, int pageSize = 10)
        {
            var lstDish = _homeRepo.GetListAllDish();

            model.lstAllDish = lstDish;
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

        [HttpGet]
        public JsonResult GetQuantityNoti()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            var rs = _nc.GetNotify(user.id);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Chat()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetNotificationCoutByUserId(int id)
        {
            var rs = _nc.GetNotify(id);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNotiNewFirst()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            var rs = _nc.GetNotifyNewFirst(user.id);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpdateNotiSeenAfterClick(int id)
        {
            var rs = _nc.UpdateNotiSeenAfterClick(id);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public List<Notify> GetAllNotifyByUserId(int id)
        {
            var rs = _nc.GetAllNotifyByUserId(id);
            return rs;
        }

        /// <summary>
        /// Khi người dùng click vào quả chuông thông báo lần đầu tiên thì chạy
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoadNoti(int page, int size)
        {
            LoadNotifyInfo notifyInfo = new LoadNotifyInfo();
            List<NotifyInfo> lstNotiNew = new List<NotifyInfo>();
            List<NotifyInfo> lstNotiOld = new List<NotifyInfo>();
            User user = (User)Session[CommonConstants.USER_SESSION];
            List<NotifyInfo> rs = _nc.LoadNoti(page, size, user.id) ?? new List<NotifyInfo>();
            foreach (var item in rs)
            {
                TimeSpan time = DateTime.Now.Subtract(item.CreatedDate.Value);
                item.Days = time.Days;
                item.Hours = time.Hours;
                item.Minutes = time.Minutes;
                item.Seconds = time.Seconds;
                if (time.Days == 0 && time.Hours == 0)
                {
                    lstNotiNew.Add(item);
                }
                else { lstNotiOld.Add(item); }
            }
            notifyInfo.lstNotiNew = lstNotiNew.OrderByDescending(x => x.CreatedDate).ToList();
            notifyInfo.lstNotiOld = lstNotiOld.OrderByDescending(x => x.CreatedDate).ToList();
            return Json(notifyInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TickALLReaded(int id)
        {
            var rs = _nc.TickALLReaded(id);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
        
        public Task<int> PushNotifi()
        {
            List<int> lst = new List<int>();
            List<string> loggedInUsers = (List<string>)HttpRuntime.Cache["LoggedInUsers"];
            if(loggedInUsers != null && loggedInUsers.Count > 0)
            {
                var lstId = loggedInUsers.Select(x => x.Split('-').FirstOrDefault()?.Split('_').LastOrDefault()).ToList();

                foreach (var item in lstId)
                {
                    int id = Int32.Parse(item);
                    lst.Add(id);
                }
            }
            if(lst.Count > 0)
            {
                _nc.NotifyRegister(lst);
            }
            int count = 1;
            return Task.FromResult(count);
        }
    }
}