using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public class ReportRepository : IReportRepository
    {
        DKACDbContext db = new DKACDbContext();

        public List<RegisterByPersonalInfo> GetListRegisterReport(int month, int fromDate, int toDate, int emId, string dish)
        {
            var query = from r in db.Registers
                        where r.RegisterDate.Month == month && r.RegisterDate.Day >= fromDate && r.RegisterDate.Day <= toDate && r.EmployeeId == emId
                        join e in db.Employees on r.EmployeeId equals e.id into le
                        from lr1 in le.DefaultIfEmpty()
                        join d in db.Dishes on r.DishId equals d.id into ld
                        from lr2 in ld.DefaultIfEmpty()
                        join ro in db.Rooms on lr1.RoomID equals ro.id into lro
                        from lr3 in lro.DefaultIfEmpty()
                        where string.IsNullOrEmpty(dish) || lr2.DishName.Contains(dish)
                        select new RegisterByPersonalInfo()
                        {
                            id = r.id,
                            DishId = lr2.id,
                            Dish = lr2,
                            EmployeeId = lr1.id,
                            EmployeeName = lr1.FullName,
                            RegisterCode = r.RegisterCode,
                            RoomId = lr1.RoomID,
                            RoomName = lr3.RoomName,
                            Ca = r.Ca,
                            Quantity = r.Quantity,
                            RegisterDate = r.RegisterDate,
                        };
            return query.ToList();
        }

        public List<RegisterByPersonalInfo> GetListRegisterByMonth(int month, int fromDate, int toDate)
        {
            var query = from r in db.Registers
                        where r.RegisterDate.Month == month && r.RegisterDate.Day >= fromDate && r.RegisterDate.Day <= toDate
                        join e in db.Employees on r.EmployeeId equals e.id into le
                        from lr1 in le.DefaultIfEmpty()
                        join d in db.Dishes on r.DishId equals d.id into ld
                        from lr2 in ld.DefaultIfEmpty()
                        join ro in db.Rooms on lr1.RoomID equals ro.id into lro
                        from lr3 in lro.DefaultIfEmpty()
                        select new RegisterByPersonalInfo()
                        {
                            id = r.id,
                            DishId = lr2.id,
                            Dish = lr2,
                            EmployeeId = lr1.id,
                            EmployeeName = lr1.FullName,
                            RegisterCode = r.RegisterCode,
                            RoomId = lr1.RoomID,
                            RoomName = lr3.RoomName,
                            Ca = r.Ca,
                            Quantity = r.Quantity,
                            RegisterDate = r.RegisterDate,
                        };
            return query.ToList();
        }

        public List<RegisterByPersonalInfo> GetListRegisterGroupByRoomId(int month, int fromDate, int toDate, int? roomId, string dish)
        {
            var query = from r in db.Registers
                        where r.RegisterDate.Month == month && r.RegisterDate.Day >= fromDate && r.RegisterDate.Day <= toDate
                        join e in db.Employees on r.EmployeeId equals e.id into le
                        from lr1 in le.DefaultIfEmpty()
                        join d in db.Dishes on r.DishId equals d.id into ld
                        from lr2 in ld.DefaultIfEmpty()
                        join ro in db.Rooms on lr1.RoomID equals ro.id into lro
                        from lr3 in lro.DefaultIfEmpty()
                        where lr3.id == roomId && lr2.DishName == dish
                        select new RegisterByPersonalInfo()
                        {
                            id = r.id,
                            DishId = lr2.id,
                            Dish = lr2,
                            EmployeeId = lr1.id,
                            EmployeeName = lr1.FullName,
                            RegisterCode = r.RegisterCode,
                            RoomId = lr1.RoomID,
                            RoomName = lr3.RoomName,
                            Ca = r.Ca,
                            Quantity = r.Quantity,
                            RegisterDate = r.RegisterDate,
                        };
            return query.ToList();
        }

        public PermissionActionInfo GetPermissionActionInfo()
        {
            var perInfo = new PermissionActionInfo();
            perInfo.ListPage = db.Pages.ToList();
            if (perInfo.ListPage == null) perInfo.ListPage = new List<Page>();
            perInfo.ListRole = db.Roles.ToList();
            if (perInfo.ListRole == null) perInfo.ListRole = new List<Role>();
            perInfo.ListAction = db.Actions.ToList();
            if (perInfo.ListAction == null) perInfo.ListAction = new List<Models.EntityModel.Action>();
            perInfo.ListModul = db.Moduls.ToList();
            if (perInfo.ListModul == null) perInfo.ListModul = new List<Modul>();
            perInfo.ListPermissionActions = db.PermissionActions.ToList();
            if (perInfo.ListPermissionActions == null) perInfo.ListPermissionActions = new List<PermissionAction>();
            return perInfo;
        }

        public int SavePermission(PermissionAction model)
        {
            var perAction = db.PermissionActions.FirstOrDefault(x => x.PageId == model.PageId && x.RoleId == model.RoleId);
            if (perAction != null)
            {
                perAction.ActionKey = model.ActionKey;
            }
            else
            {
                db.PermissionActions.Add(model);
            }
            return db.SaveChanges();
        }

        public List<Role> GetAllRole()
        {
            var Roles = db.Roles.ToList();
            if (Roles == null) { Roles = new List<Role>(); }
            return Roles;
        }

        public List<User> GetAllUser()
        {
            var Users = db.Users.ToList();
            if (Users == null) { Users = new List<User>(); }
            return Users;
        }

        public List<UserRole> GetAllUserRole()
        {
            var ur = db.UserRoles.ToList();
            if (ur == null) ur = new List<UserRole>();
            return ur;
        }

        public int SaveUserRole(UserRoleInfo model)
        {
            var userRole = db.UserRoles.FirstOrDefault(x => x.UserId == model.UserId && x.RoleId == model.RoleId);
            var role = db.Roles.FirstOrDefault(x => x.Id == model.RoleId);
            var uR = new UserRole();
            uR.RoleId = model.RoleId;
            uR.UserId = model.UserId;
            uR.Description = role?.Description;
            if (model.Check == true)
            {
                db.UserRoles.Add(uR);
            }
            if (model.Check == false && userRole != null)
            {
                db.UserRoles.Remove(userRole);
            }
            return db.SaveChanges();
        }

        public int SaveRole(Role model)
        {
            var uR = new Role();
            uR.Id = model.Id;
            uR.RoleName = model.RoleName;
            uR.Description = model.Description;
            db.Roles.Add(uR);
            return db.SaveChanges();
        }
    }
}