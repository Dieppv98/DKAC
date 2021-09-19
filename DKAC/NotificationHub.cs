using DKAC.Common;
using DKAC.Controllers;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DKAC
{
    public class NotificationHub : Hub
    {
        //public ConcurrentDictionary<int, string> messageUsers = new ConcurrentDictionary<int, string>();
        HomeController _home = new HomeController();

        [HubMethodName("conectionServer")]
        public static void ConectionServer()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.conectionServer();
        }

        public void SendMessageTo(User userSend, string message, int? receiver)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            if (userSend.UserGroupId == (int)GroupUser.admin)
            {
                ConcurrentDictionary<int, string> messageUsers = (ConcurrentDictionary<int, string>)HttpRuntime.Cache["MessageUsers"];
                if (messageUsers != null && messageUsers.Count > 0)
                {
                    var receive = messageUsers.FirstOrDefault(x => x.Key == receiver);
                    if(receive.Key != 0 && receive.Value != null)
                    {
                        //Clients.Caller.getmessage(receive.Value, message, -1);
                        context.Clients.Client(receive.Value).getmessage("getmessageclient", message, -1);
                    }
                }
            }
            else
            {
                //gửi cho tất cả các admin userSend.id là người gửi message cho admin
                context.Clients.All.getmessage("getmessageadmin", message, userSend.id);
            }
        }

        public void online(int userId, string hubConecId)
        {
            _home.UserOnline(userId, hubConecId);
        }
    }
}