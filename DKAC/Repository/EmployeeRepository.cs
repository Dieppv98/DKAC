using DKAC.Common;
using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        DKACDbContext db = new DKACDbContext();
        public UserRequestModel GetListAllEmployee(User user, string KeySearch, int page, int pageSize)
        {
            UserRequestModel request = new UserRequestModel();
            List<User> lst = new List<User>();
            if (user.UserGroupId == (int)GroupUser.admin)
            {
                lst = db.Users.Where(x => x.IsDeleted == 0).ToList();
            }
            else
            {
                //if (user.Role == 2)
                //{
                //    lst = db.Users.Where(x => x.RoomID == user.RoomID && x.IsDeleted == 0).ToList();
                //}
                lst = db.Users.Where(x => x.RoomID == user.RoomID && x.IsDeleted == 0).ToList();
            }
            request.totalRecord = lst.Count;
            request.page = page;
            request.pageSize = pageSize;
            int startRow = (page - 1) * pageSize;
            if (!string.IsNullOrEmpty(KeySearch))
            {
                request.data = lst.Where(x => x.IsDeleted == 0 && x.FullName.Contains(KeySearch) || x.UserName.Contains(KeySearch)).OrderBy(x => x.id).Skip(startRow).Take(pageSize).ToList();
            }
            else
            {
                request.data = lst.OrderBy(x => x.UserName).Skip(startRow).Take(pageSize).ToList();
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

        public int Add(User employee)
        {
            try
            {
                employee.CreatedDate = DateTime.Now;
                employee.ModifyDate = DateTime.Now;
                db.Users.Add(employee);
                db.SaveChanges();
                var emp = db.Users.Where(x => x.RoomID == employee.RoomID && x.IsDeleted == 0).ToList();
                var room = db.Rooms.Where(x => x.id == employee.RoomID && x.IsDeleted == 0).FirstOrDefault();
                room.Members = emp.Count;
                room.ModifyDate = DateTime.Now;
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public int Delete(int id)
        {
            var data = db.Users.Where(x => x.id == id).FirstOrDefault();
            var room = db.Rooms.Where(x => x.id == data.RoomID && x.IsDeleted == 0).FirstOrDefault();
            if (data == null) { return 0; }
            data.ModifyDate = DateTime.Now;
            data.IsDeleted = 1;
            db.SaveChanges();
            var emp = db.Users.Where(x => x.RoomID == room.id && x.IsDeleted == 0).ToList();
            room.Members = emp.Count;
            db.SaveChanges();
            return 1;
        }

        public UserInfo GetById(int? id)
        {
            var data = db.Users.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault();
            var user = (from u in db.Users
                        where u.IsDeleted == 0 && u.id == id
                        join r in db.Rooms on u.RoomID equals r.id into lr
                        from lru in lr.DefaultIfEmpty()
                        select new UserInfo()
                        {
                            id = u.id,
                            FullName = u.FullName,
                            PassWord = "",
                            UserName = u.UserName,
                            RoomID = u.RoomID,
                            Role = u.Role,
                            Code = u.Code,
                            Position = u.Position,
                            UserGroupId = u.UserGroupId,
                            RoomName = lru.RoomName
                        }).FirstOrDefault() ?? new UserInfo();
            return user;
        }

        public User GetByUserName(string UserName, int? id)
        {
            var data = db.Users.Where(x => x.IsDeleted == 0 && x.id != id && x.UserName == UserName).FirstOrDefault();
            return data;
        }

        public List<Room> GetAllRoom()
        {
            return db.Rooms.Where(x => x.IsDeleted == 0).ToList();
        }

        public int Update(UserInfo employee)
        {
            var data = db.Users.Where(x => x.id == employee.id).FirstOrDefault();
            if (data == null) { return 0; }
            int? roomId = data.RoomID;//phòng cũ

            var data1 = db.Users.Where(x => x.UserName == employee.UserName && x.IsDeleted == 1 && x.id != employee.id).FirstOrDefault();//cherck trung username phát nữa khi lưu
            if (data1 != null) { return 0; }
            
            data.FullName = employee.FullName;
            data.UserName = employee.UserName;
            data.PassWord = employee.PassWord;
            data.Role = employee.Role;
            data.UserGroupId = employee.UserGroupId;
            data.RoomID = employee.RoomID;
            data.IsDeleted = 0;
            db.SaveChanges();
            if (employee.RoomID != null)
            {
                //cập nhật số nhân viên ở phòng mới
                var room = db.Rooms.Where(x => x.id == employee.RoomID && x.IsDeleted == 0).FirstOrDefault();
                var lstUByRoom = db.Users.Where(x => x.RoomID == employee.RoomID && x.IsDeleted == 0).ToList();
                room.Members = lstUByRoom.Count;

                //cập nhật số nhân viên ở phòng cũ
                var room1 = db.Rooms.First(x => x.id == roomId && x.IsDeleted == 0);
                var lstUByRoom1 = db.Users.Where(x => x.RoomID == roomId && x.IsDeleted == 0).ToList();
                room1.Members = lstUByRoom1.Count;
                db.SaveChanges();
            }
            return 1;
        }

        public List<User> GetEmployees(int? roomId)
        {
            if (roomId == null)
            {
                return db.Users.Where(x => x.IsDeleted == 0).ToList();
            }
            return db.Users.Where(x => x.IsDeleted == 0 && x.RoomID == roomId).ToList();
        }

        public List<UserInfo> GetEmInfo(int? roomId)
        {
            var query = from e in db.Users
                        where e.IsDeleted == 0 && e.RoomID == roomId
                        join r in db.Rooms on e.RoomID equals r.id into le
                        from lr in le.DefaultIfEmpty()
                        select new UserInfo()
                        {
                            id = e.id,
                            Code = e.Code,
                            FullName = e.FullName,
                            RoomID = e.RoomID,
                            RoomName = lr.RoomName,
                            Role = e.Role,
                            UserName = e.UserName,
                            UserGroupId = e.UserGroupId,
                            Position = e.Position,
                        };
            return query.ToList();
        }

        public List<Dish> GetAllDish()
        {
            return db.Dishes.Where(x => x.IsDeleted == 0).ToList();
        }

    }
}