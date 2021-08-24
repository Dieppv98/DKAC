using DKAC.Models.EntityModel;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using DKAC.Common;

namespace DKAC
{
    public class NotificationComponent
    {
        DKACDbContext db = new DKACDbContext();

        public void NotifyRegister(List<int> lst)
        {
            List<Notify> lstNoti = new List<Notify>();
            var lstU = db.Users.Where(x => x.IsDeleted == 0 && lst.Contains(x.id)).ToList() ?? new List<User>();
            foreach (var item in lstU)
            {
                Notify notify = new Notify()
                {
                    ReceiveUserId = item.id,
                    SeenStatus = null,
                    TypeNoti = (int)TypeOfNoti.TypeRegister,
                    CreatedDate = DateTime.Now,
                    ContentNoti = $"{item.FullName.Split(' ')?.LastOrDefault() ?? "Bạn"} ơi! Vào đăng ký món ăn đi nào!!!"
                };
                lstNoti.Add(notify);
            }
            if(lstNoti.Count > 0)
            {
                db.Notifies.AddRange(lstNoti);
                db.SaveChanges();
            }

            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            notificationHub.Clients.All.notify("notify");
        }

        public void NotifyAddNewDish(int id)
        {
            var lstU = AllUsers();
            var dish = db.Dishes.Where(x => x.id == id).FirstOrDefault();
            if (lstU.Count > 0)
            {
                foreach (var item in lstU)
                {
                    Notify notify = new Notify()
                    {
                        ReceiveUserId = item.id,
                        SeenStatus = null,
                        TypeNoti = (int)TypeOfNoti.TypeDishNew,
                        CreatedDate = DateTime.Now,
                        ContentNoti = "Món mới dành cho bạn. Hãy vào xem ngay nào!",
                        Image = dish.Image,
                        DishId = id,
                    };
                    db.Notifies.Add(notify);
                }
                db.SaveChanges();
            }

            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            notificationHub.Clients.All.notify("notifyaddnewdish");
        }
        
        public List<User> AllUsers()
        {
            return db.Users.Where(x => x.IsDeleted == 0).ToList();
        }
        
        public List<Notify> GetNotify(int id)
        {
            return db.Notifies.Where(x => x.SeenStatus != (int)SeenStatus.Seen && x.ReceiveUserId == id).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public Notify GetNotifyNewFirst(int id)
        {
            return db.Notifies.Where(x => x.ReceiveUserId == id).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        public int UpdateNotiSeenAfterClick(int id)
        {
            var noti = db.Notifies.Where(x => x.Id == id).FirstOrDefault();
            if (noti != null)
                noti.SeenStatus = (int)SeenStatus.Seen;
            return db.SaveChanges();
        }

        public List<Notify> GetAllNotifyByUserId(int id)
        {
            return db.Notifies.Where(x => x.ReceiveUserId == id).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<NotifyInfo> LoadNoti(int page, int size, int userId)
        {
            page = (page - 1);
            var data = (from n in db.Notifies
                        where n.ReceiveUserId == userId
                        select new NotifyInfo()
                        {
                            Id = n.Id,
                            ReceiveUserId = n.ReceiveUserId,
                            CreatedDate = n.CreatedDate,
                            ContentNoti = n.ContentNoti,
                            DishId = n.DishId,
                            Image = n.Image,
                            SeenStatus = n.SeenStatus,
                            TypeNoti = n.TypeNoti,
                        }).OrderByDescending(x => x.CreatedDate).Skip(page * size).Take(size).ToList() ?? new List<NotifyInfo>();

            return data;
        }

        public int TickALLReaded(int id)
        {
            var lstNoti = db.Notifies.Where(x => x.SeenStatus != (int)SeenStatus.Seen && x.ReceiveUserId == id).ToList();
            foreach (var item in lstNoti)
            {
                item.SeenStatus = (int)SeenStatus.Seen;
            }

            return db.SaveChanges();
        }
    }
}