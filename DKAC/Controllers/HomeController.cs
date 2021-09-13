using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using DKAC.Repository;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RabbitMQ.Client;
using DKAC.Models.Enum;
using System.Collections.Concurrent;

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
                item.Hour_Minute = $"{item.CreatedDate.Value.Hour}_{item.CreatedDate.Value.Minute}";
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
            _nc.NotifyRegister();

            int count = 1;
            return Task.FromResult(count);
        }

        //Cái này là sử dụng database để lưu user on
        [HttpGet]
        public Task<JsonResult> CheckUserOnline()
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            var rs = _nc.CheckUserOnline(user);

            return Task.FromResult(Json(rs, JsonRequestBehavior.AllowGet));
        }

        /// <summary>
        /// cập nhật user online khi user mở ứng dụng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<JsonResult> UpdateOnlineUserOnload()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            if (HttpRuntime.Cache["OnlineUsers"] != null)
            {
                Dictionary<int, DateTime> onlineUsers = (Dictionary<int, DateTime>)HttpRuntime.Cache["OnlineUsers"];
                var online = onlineUsers.Where(x => x.Key == user.id).FirstOrDefault();
                if (online.Key <= 0)
                {
                    onlineUsers.Add(user.id, DateTime.Now);
                    HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                }
                else
                {
                    onlineUsers.Remove(online.Key);
                    onlineUsers.Add(user.id, DateTime.Now);
                    HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                }
            }
            else
            {
                Dictionary<int, DateTime> onlineUsers = new Dictionary<int, DateTime>();
                onlineUsers.Add(user.id, DateTime.Now);
                HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
            }
            
            var rs = receive(user);
            return Task.FromResult(Json(rs, JsonRequestBehavior.AllowGet));
        }

        public JsonResult receive(User user)
        {
            try
            {
                string userqueue = "-1";
                if (user.UserGroupId != 3)//nếu nhóm khác admin thì người nhận = id của user else = -1
                    userqueue = user.id.ToString();

                RabbitMQB obj = new RabbitMQB();
                RabbitMQ.Client.IConnection con = obj.GetConnection();

                string message = obj.receive(con, userqueue);
                return Json(message);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// cập nhật user offline khi tắt tab hoặc đóng trình duyệt
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<int> UpdateOfflineUserClose()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            //kiểm tra xem đã có cache chưa
            if (HttpRuntime.Cache["OnlineUsers"] != null)
            {
                Dictionary<int, DateTime> onlineUsers = (Dictionary<int, DateTime>)HttpRuntime.Cache["OnlineUsers"];
                var online = onlineUsers.Where(x => x.Key == user.id).FirstOrDefault(); //lần user vừa đóng tab, thoát trình duyệt
                if (online.Key > 0)
                {
                    onlineUsers.Remove(online.Key); //remove đi
                    HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                }
            }

            if(user.UserGroupId != (int)GroupUser.admin)
            {
                if (HttpRuntime.Cache["MessageUsers"] != null)
                {
                    Dictionary<int, string> messageUsers = (Dictionary<int, string>)HttpRuntime.Cache["MessageUsers"];
                    var online = messageUsers.Where(x => x.Key == user.id).FirstOrDefault(); //lần user vừa đóng tab, thoát trình duyệt
                    if (online.Key > 0)
                    {
                        messageUsers.Remove(online.Key); //remove đi
                        HttpRuntime.Cache["MessageUsers"] = messageUsers;
                    }
                }
            }
            int count = 1;
            return Task.FromResult(count);
        }

        /// <summary>
        /// refresh lại list user đang online sau mỗi khoảng thời gian
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<JsonResult> RefreshListUserOnline()
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            var now = DateTime.Now.AddMinutes(-2);

            Dictionary<int, string> messageUsers = (Dictionary<int, string>)HttpRuntime.Cache["MessageUsers"];

            if (HttpRuntime.Cache["OnlineUsers"] != null)
            {
                Dictionary<int, DateTime> onlineUsers = (Dictionary<int, DateTime>)HttpRuntime.Cache["OnlineUsers"];
                var lstonline = onlineUsers.Where(x => x.Value <= now).ToList();
                foreach (var item in lstonline)
                {
                    onlineUsers.Remove(item.Key);
                }
                var userParam = onlineUsers.FirstOrDefault(x => x.Key == user.id);
                if(userParam.Key <= 0)
                {
                    onlineUsers.Add(user.id, DateTime.Now);
                    HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                }
                else
                {
                    onlineUsers.Remove(userParam.Key);
                    onlineUsers.Add(user.id, DateTime.Now);
                    HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
                }
                return Task.FromResult(Json(1, JsonRequestBehavior.AllowGet));
            }
            else
            {
                Dictionary<int, DateTime> onlineUsers = new Dictionary<int, DateTime>();
                onlineUsers.Add(user.id, DateTime.Now);
                HttpRuntime.Cache["OnlineUsers"] = onlineUsers;
            }
            return Task.FromResult(Json(1, JsonRequestBehavior.AllowGet));
        }

        public void UserOnline(int userId, string hubConecId)
        {
            if (userId != -1)
            {
                if (HttpRuntime.Cache["MessageUsers"] != null)
                {
                    Dictionary<int, string> messageUsers = (Dictionary<int, string>)HttpRuntime.Cache["MessageUsers"];
                    var cache = messageUsers.Where(x => x.Key == userId).FirstOrDefault();
                    if (cache.Key <= 0) //kiểm tra nếu trong cache chưa có thì mới add vào
                    {
                        messageUsers.Add(userId, hubConecId);
                        HttpRuntime.Cache["MessageUsers"] = messageUsers;
                    }
                    else
                    {
                        messageUsers.Remove(cache.Key);
                        messageUsers.Add(userId, hubConecId);
                        HttpRuntime.Cache["MessageUsers"] = messageUsers;
                    }
                }
                else
                {
                    Dictionary<int, string> messageUsers = new Dictionary<int, string>();
                    messageUsers.Add(userId, hubConecId);
                    HttpRuntime.Cache["MessageUsers"] = messageUsers;
                }
            }
        }
    }
}