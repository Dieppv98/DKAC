using DKAC.Common;
using DKAC.Controllers;
using DKAC.Models.EntityModel;
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
        static ConcurrentDictionary<int, string> dic = new ConcurrentDictionary<int, string>();
        HomeController _home = new HomeController();
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}

        [HubMethodName("conectionServer")]
        public static void ConectionServer()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.conectionServer();
        }

        public void online(int userId, string hubConecId)
        {
        }

        public Task disconnected()
        {
            _home.Test();

            int count = 1;
            return Task.FromResult(count);
        }
    }
}