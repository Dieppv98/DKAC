using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public class RoomRepository : IRoomRepository
    {
        DKACDbContext db = new DKACDbContext();
        public RoomRequestModel GetListAllRoom(RoomRequestModel request, int pageIndex, int recordPerPage, out int totalCount)
        {
            pageIndex = pageIndex - 1;
            if (!string.IsNullOrEmpty(request.Keywords))
            {
                request.Keywords = request.Keywords.Trim();
            }

            var query = from r in db.Rooms
                        join u in db.Users on r.Manager equals u.id into lu
                        from lur in lu.DefaultIfEmpty()
                        where (string.IsNullOrEmpty(request.Keywords) || r.RoomName.Contains(request.Keywords) || r.RoomShortName.Contains(request.Keywords))
                        select new RoomInfo()
                        {
                            Id = r.id,
                            Manager = r.Manager,
                            ManagerName = lur.FullName,
                            RoomName = r.RoomName,
                            RoomShortName = r.RoomShortName,
                            Members = r.Members,
                            DiaChi = r.DiaChi,
                            SDT = r.SDT,
                        };
            totalCount = query.Count();
            var rs = query.OrderBy(x => x.Id).Skip(pageIndex * recordPerPage).Take(recordPerPage).ToList() ?? new List<RoomInfo>();
            request.data = rs;
            return request ?? new RoomRequestModel();
        }


        public int Add(Room room)
        {
            try
            {
                room.CreatedDate = DateTime.Now;
                room.ModifyDate = DateTime.Now;
                room.Members = room.Members ?? 0;
                db.Rooms.Add(room);
                db.SaveChanges();
                return 1;
            }

            catch (Exception)
            {

                return 0;
            }
        }

        public int Delete(int id)
        {
            var data = db.Rooms.Where(x => x.id == id).FirstOrDefault();
            var employee = db.Employees.Where(x => x.RoomID == data.id && x.IsDeleted == 0).ToList();
            if (data == null)
            {
                return 0;
            }
            data.ModifyDate = DateTime.Now;
            data.IsDeleted = 1;
            foreach (var item in employee)
            {
                item.IsDeleted = 1;
            }
            db.SaveChanges();
            return 1;
        }

        public Room GetById(int id)
        {
            var data = db.Rooms.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault();
            if (data == null)
            {
                return new Room();
            }
            return data;
        }

        public int Update(Room room)
        {
            var data = db.Rooms.Where(x => x.id == room.id).FirstOrDefault();
            var data1 = db.Rooms.Where(x => x.RoomName == room.RoomName && x.IsDeleted == 1).FirstOrDefault();
            if (data == null) return 0;
            if (data.RoomName != room.RoomName && data1 != null) return 0;
            if (room.Manager != null)
            {
                var lstEm = db.Users.Where(x => x.RoomID == data.id && x.IsDeleted == 0).ToList();
                foreach (var item in lstEm)
                {
                    item.Role = 1;
                }
                var em = db.Users.FirstOrDefault(x => x.IsDeleted == 0 && x.id == room.Manager);
                em.Role = 2;
                data.Manager = room.Manager;
            }
            data.RoomName = room.RoomName;
            data.RoomShortName = room.RoomShortName;
            data.ModifyDate = DateTime.Now;

            db.SaveChanges();
            return 1;
        }

        public Room GetByShortName(string RoomShortName, int? id)
        {
            var data = db.Rooms.Where(x => x.IsDeleted == 0 && x.id != id && x.RoomShortName == RoomShortName).FirstOrDefault();
            return data;
        }

        public Room GetByRoomName(string RoomName, int? id)
        {
            var data = db.Rooms.Where(x => x.IsDeleted == 0 && x.id != id && x.RoomName == RoomName).FirstOrDefault();
            return data;
        }

        public List<User> GetAllEmployeeByRoom(int roomId)
        {
            return db.Users.Where(x => x.IsDeleted == 0 && x.RoomID == roomId).ToList();
        }

        public string GetRoomNameByRoomId(int? id)
        {
            var data = db.Rooms.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault();
            return data.RoomName;
        }
        public List<User> GetAllMasterRoom()
        {
            return db.Users.Where(x => x.IsDeleted == 0 && x.Role == 2).ToList();
        }

    }
}