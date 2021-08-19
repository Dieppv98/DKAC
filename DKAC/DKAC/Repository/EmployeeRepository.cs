using DKAC.Common;
using DKAC.IRepository;
using DKAC.Models.EntityModel;
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
        public EmployeeRequestModel GetListAllEmployee(Employee employee, User user, string KeySearch, int page, int pageSize)
        {
            EmployeeRequestModel request = new EmployeeRequestModel();
            List<Employee> lst = new List<Employee>();
            if (user != null)
            {
                lst = db.Employees.Where(x => x.IsDeleted == 0).ToList();
            }
            else if (employee.Role == 2)
            {
                lst = db.Employees.Where(x => x.RoomID == employee.RoomID && x.IsDeleted == 0).ToList();
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

        public int Add(Employee employee)
        {
            try
            {
                employee.CreatedDate = DateTime.Now;
                employee.ModifyDate = DateTime.Now;
                db.Employees.Add(employee);
                db.SaveChanges();
                var emp = db.Employees.Where(x => x.RoomID == employee.RoomID && x.IsDeleted == 0).ToList();
                var room = db.Rooms.Where(x => x.id == employee.RoomID && x.IsDeleted == 0).FirstOrDefault();
                room.Members = emp.Count;
                db.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public int Delete(int id)
        {
            var data = db.Employees.Where(x => x.id == id).FirstOrDefault();
            var room = db.Rooms.Where(x => x.id == data.RoomID && x.IsDeleted == 0).FirstOrDefault();
            if (data == null) { return 0; }
            data.ModifyDate = DateTime.Now;
            data.IsDeleted = 1;
            db.SaveChanges();
            var emp = db.Employees.Where(x => x.RoomID == room.id && x.IsDeleted == 0).ToList();
            room.Members = emp.Count;
            db.SaveChanges();
            return 1;
        }

        public Employee GetById(int? id)
        {
            var data = db.Employees.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault();
            if (data == null) { return new Employee(); }
            return data;
        }

        public Employee GetByUserName(string UserName, int? id)
        {
            var data = db.Employees.Where(x => x.IsDeleted == 0 && x.id != id && x.UserName == UserName).FirstOrDefault();
            return data;
        }

        public List<Room> GetAllRoom()
        {
            return db.Rooms.Where(x => x.IsDeleted == 0).ToList();
        }

        public int Update(Employee employee)
        {
            var data = db.Employees.Where(x => x.id == employee.id).FirstOrDefault();
            Employee emp1 = db.Employees.First(x => x.RoomID == data.RoomID && x.IsDeleted == 0);
            int? roomId = emp1.RoomID;
            if (emp1 == null) { return 0; }
            var data1 = db.Employees.Where(x => x.UserName == employee.UserName && x.IsDeleted == 1).FirstOrDefault();
            if (data == null) { return 0; }
            if (data.UserName != employee.UserName && data1 != null) { return 0; }
            data.id = employee.id;
            data.FullName = employee.FullName;
            data.UserName = employee.UserName;
            data.PassWord = employee.PassWord;
            data.Birthday = employee.Birthday;
            data.Gender = employee.Gender;
            data.CMND = employee.CMND;
            data.Email = employee.Email;
            data.Tel = employee.Tel;
            data.Address = employee.Address;
            data.IsDeleted = 0;
            db.SaveChanges();
            if (employee.RoomID != null)
            {
                var emp = db.Employees.Where(x => x.RoomID == employee.RoomID && x.IsDeleted == 0).ToList();
                if (emp == null) { return 0; }
                var room = db.Rooms.Where(x => x.id == employee.RoomID && x.IsDeleted == 0).FirstOrDefault();
                room.Members = emp.Count;
                var emp2 = db.Employees.Where(x => x.RoomID == roomId && x.IsDeleted == 0).ToList();
                if (emp2 == null) { return 0; }
                var room1 = db.Rooms.First(x => x.id == roomId && x.IsDeleted == 0);
                room1.Members = emp2.Count;
                db.SaveChanges();
            }
            return 1;
        }

        public List<Employee> GetEmployees(int? roomId)
        {
            if (roomId == null)
            {
                return db.Employees.Where(x => x.IsDeleted == 0).ToList();
            }
            return db.Employees.Where(x => x.IsDeleted == 0 && x.RoomID == roomId).ToList();
        }

        public List<EmployeeInfo> GetEmInfo(int? roomId)
        {
            var query = from e in db.Employees
                        where e.IsDeleted == 0 && e.RoomID == roomId
                        join r in db.Rooms on e.RoomID equals r.id into le
                        from lr in le.DefaultIfEmpty()
                        select new EmployeeInfo()
                        {
                            id = e.id,
                            Address = e.Address,
                            Birthday = e.Birthday,
                            CMND = e.CMND,
                            Email = e.Email,
                            FullName = e.FullName,
                            RoomID = e.RoomID,
                            RoomName = lr.RoomName,
                        };
            return query.ToList();
        }

    }
}