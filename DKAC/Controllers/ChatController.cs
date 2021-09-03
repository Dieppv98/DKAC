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
        //public static ChatRepository chat = new ChatRepository();
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
        public JsonResult GetChatByClinet(int page, int size)
        {
            //_chat = chat;
            User user = (User)Session[CommonConstants.USER_SESSION];
            var rs = _chat.GetChatByClinet(page, size, user.id);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> sendmsg(ChatReceiverInfo model)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            try
            {
                var end = "-1";
                if (user.UserGroupId == (int)GroupUser.admin)
                {
                    end = model.ReceiverId.ToString() ?? "-1";
                    model.SenderId = -1;// nếu là admin gửi thì gán người gửi = -1;
                }
                else
                {
                    model.SenderId = user.id;
                    model.ReceiverId = -1;
                }//gán id của người gửi
                RabbitMQB obj = new RabbitMQB();
                IConnection con = obj.GetConnection();
                bool flag = obj.send(con, model.Message, end);
                if (flag == true)
                {
                    var rs = await _chat.AddMessage(model);
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
                RabbitMQB obj = new RabbitMQB();
                IConnection con = obj.GetConnection();
                string userqueue = Session["username"].ToString();
                string message = obj.receive(con, userqueue);
                return Json(message);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }


        }
    }
}