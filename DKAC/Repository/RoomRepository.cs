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
        public RoomRequestModel GetListAllRoom(Employee employee, User user, string KeySearch, int page, int pageSize)
        {
            RoomRequestModel request = new RoomRequestModel();
            List<Room> lst = new List<Room>();
            if (user != null)
            {
                lst = db.Rooms.Where(x => x.IsDeleted == 0).ToList();
            }
            else if (employee.Role == 2)
            {
                lst = db.Rooms.Where(x => x.IsDeleted == 0 && x.id == employee.RoomID).ToList();
            }
            request.totalRecord = lst.Count;
            request.page = page;
            request.pageSize = pageSize;
            int startRow = (page - 1) * pageSize;
            if (!string.IsNullOrEmpty(KeySearch))
            {
                request.data = lst.Where(x => x.IsDeleted == 0 && x.RoomName.Contains(KeySearch) || x.RoomShortName.Contains(KeySearch)).OrderBy(x => x.id).Skip(startRow).Take(pageSize).ToList();
            }
            else
            {
                request.data = lst.OrderBy(x => x.RoomShortName).Skip(startRow).Take(pageSize).ToList();
            }
            int totalPage = 0;
            if (request.totalRecord % pageSize == 0)
            {
                totalPage = request.totalRecord / pageSize;
            }
            else
            {
                totalPage = request.totalRecord / pageSize + 1;
            }
            request.totalPage = totalPage;
            return request;
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
                var lstEm = db.Employees.Where(x => x.RoomID == data.id && x.IsDeleted == 0).ToList();
                foreach (var item in lstEm)
                {
                    item.Role = 1;
                }
                var em = db.Employees.FirstOrDefault(x => x.IsDeleted == 0 && x.id == room.Manager);
                em.Role = 2;
                data.Manager = room.Manager;
            }
            data.RoomName = room.RoomName;
            data.RoomShortName = room.RoomShortName;
            data.IsDeleted = 0;
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