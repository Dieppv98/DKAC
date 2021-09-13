using DKAC.Common;
using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using DKAC.Repository;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Controllers
{
    //[Authorize]
    //[AuthenticateUser]
    public class ChatController : BaseController
    {
        public static IChatRepository _chat = new ChatRepository();
        public static NotificationHub _hub = new NotificationHub();
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetReceiverChat()
        {
            //_chat = chat;
            User user = (User)Session[CommonConstants.USER_SESSION];
            var rs = _chat.GetReceiverChatById(user.id);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChatByClient(int page, int size, int toId)
        {
            //_chat = chat;
            User user = (User)Session[CommonConstants.USER_SESSION];
            var rs = _chat.GetChatByClient(page, size, user.id, toId);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> sendmsg(ChatReceiverInfo model)
        {
            User userSend = (User)Session[CommonConstants.USER_SESSION];
            try
            {
                var end = "-1";
                if (userSend.UserGroupId == (int)GroupUser.admin)
                {
                    end = model.ReceiverId.ToString() ?? "-1";
                    model.SenderId = (int)MessageOfAdmin.admin;// nếu là admin gửi thì gán người gửi = -1;
                }
                else
                {
                    model.SenderId = userSend.id;
                    model.ReceiverId = (int)MessageOfAdmin.admin;
                }//gán id của người gửi
                RabbitMQB obj = new RabbitMQB();
                IConnection con = obj.GetConnection();
                bool flag = obj.send(con, model.Message, end);
                if (flag == true)
                {
                    var rs = await _chat.AddMessage(model);
                    //phát tín hiệu cho client biết mà get message về
                    if(rs > 0)
                    {
                        _hub.SendMessageTo(userSend, model.Message, model.ReceiverId);
                    }
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                throw;
            }
        }

        [HttpPost]
        public JsonResult receive()
        {
            try
            {
                string userqueue = "-1";
                User user = (User)Session[CommonConstants.USER_SESSION];
                if (user.UserGroupId != 3)//nếu nhóm khác admin thì người nhận = id của user else = -1
                    userqueue = user.id.ToString();

                RabbitMQB obj = new RabbitMQB();
                IConnection con = obj.GetConnection();
                List<string> lstMessage = new List<string>();
                string message = "";
                //lấy hết message queu của người nhận hiện tại đang request
                while ((message != null))
                {
                    string item = obj.receive(con, userqueue);
                    if (item != null)
                        lstMessage.Add(item);
                    message = item;
                }
                
                return Json(lstMessage);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Chức năng này chỉ dành cho admin
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadPopupSelectChat()
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            List<SelectChatInfo> rs = new List<SelectChatInfo>();
            if (user.UserGroupId == (int)GroupUser.admin)
            {
                var now = DateTime.Now;
                rs = _chat.LoadPopupSelectChat();

                foreach (var item in rs)
                {
                    TimeSpan time = now.Subtract(item.CreatedDate.Value);
                    if(time.Days > 0) item.TimeDisplay = $"{time.Days} ngày trước";
                    else if(time.Days == 0 && time.Hours > 0) item.TimeDisplay = $"{time.Hours} giờ trước";
                    else if(time.Days == 0 && time.Hours == 0 && time.Minutes > 0) item.TimeDisplay = $"{time.Minutes} phút trước";
                    else if(time.Days == 0 && time.Hours == 0 && time.Minutes == 0 && time.Seconds > 0) item.TimeDisplay = $"{time.Seconds} giây trước";
                }
            }
            
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Chức năng của admin, khi mở popup chát của ai thì đánh dấu là đã xem
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateSeenById(int updateSeeById)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            if (user.UserGroupId == (int)GroupUser.admin)
            {
                var rs = _chat.UpdateSeenById(updateSeeById);
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}