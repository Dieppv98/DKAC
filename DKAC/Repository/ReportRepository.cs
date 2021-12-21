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

        public List<RegisterByPersonalInfo> GetListRegisterReport(DateTime? fDate, DateTime? tDate, int? emId, string dish)
        {
            var query = from r in db.Registers
                        where r.UserId == emId && ((!fDate.HasValue || r.RegisterDate >= fDate) && (!tDate.HasValue || r.RegisterDate <= tDate))
                        join e in db.Users on r.UserId equals e.id into le
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
                            UserId = lr1.id,
                            EmployeeName = lr1.FullName,
                            RegisterCode = r.RegisterCode,
                            RoomId = lr1.RoomID,
                            RoomName = lr3.RoomName,
                            Ca = r.Ca,
                            Quantity = r.Quantity,
                            RegisterDate = r.RegisterDate,
                        };
            return query.OrderByDescending(x => x.RegisterDate).ToList();
        }

        public List<RegisterByPersonalInfo> GetListRegisterByMonth(DateTime? fDate, DateTime? tDate, int? roomId)
        {
            var query = from r in db.Registers
                        where ((!fDate.HasValue || r.RegisterDate >= fDate) && (!tDate.HasValue || r.RegisterDate <= tDate))
                        join e in db.Users on r.UserId equals e.id into le
                        from lr1 in le.DefaultIfEmpty()
                        join d in db.Dishes on r.DishId equals d.id into ld
                        from lr2 in ld.DefaultIfEmpty()
                        join ro in db.Rooms on lr1.RoomID equals ro.id into lro
                        from lr3 in lro.DefaultIfEmpty()
                        where (!roomId.HasValue || lr3.id == roomId)
                        select new RegisterByPersonalInfo()
                        {
                            id = r.id,
                            DishId = lr2.id,
                            Dish = lr2,
                            UserId = lr1.id,
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

        public List<RegisterByPersonalInfo> GetListRegisterGroupByRoomId(DateTime? fDate, DateTime? tDate, int? roomId, int? userId)
        {
            var query = from r in db.Registers
                        where (!userId.HasValue || r.UserId == userId) && ((!fDate.HasValue || r.RegisterDate >= fDate) && (!tDate.HasValue || r.RegisterDate <= tDate))
                        join e in db.Users on r.UserId equals e.id into le
                        from lr1 in le.DefaultIfEmpty()
                        join d in db.Dishes on r.DishId equals d.id into ld
                        from lr2 in ld.DefaultIfEmpty()
                        join ro in db.Rooms on lr1.RoomID equals ro.id into lro
                        from lr3 in lro.DefaultIfEmpty()
                        where (!roomId.HasValue || lr3.id == roomId)
                        select new RegisterByPersonalInfo()
                        {
                            id = r.id,
                            DishId = lr2.id,
                            Dish = lr2,
                            UserId = lr1.id,
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


        public List<ReportByDishInfo> GetListRegisterByDish(DateTime? fDate, DateTime? tDate, int? roomId, int? dishId)
        {
            List<ReportByDishInfo> lst = new List<ReportByDishInfo>();
            var query = (from r in db.Registers
                         where ((!fDate.HasValue || r.RegisterDate >= fDate) && (!tDate.HasValue || r.RegisterDate <= tDate))
                         join e in db.Users on r.UserId equals e.id into le
                         from lr1 in le.DefaultIfEmpty()
                         join d in db.Dishes on r.DishId equals d.id into ld
                         from lr2 in ld.DefaultIfEmpty()
                         join ro in db.Rooms on lr1.RoomID equals ro.id into lro
                         from lr3 in lro.DefaultIfEmpty()
                         where (!roomId.HasValue || lr3.id == roomId) && (!dishId.HasValue || lr2.id == dishId)
                         select new ReportByDishInfo()
                         {
                             Id = r.id,
                             DishId = lr2.id,
                             DishName = lr2.DishName,
                             RoomId = lr1.RoomID,
                             RoomName = lr3.RoomName,
                         }).ToList() ?? new List<ReportByDishInfo>();

            if(query.Count > 0)
            {
                var group = query.GroupBy(x => x.DishId).Select(g => new ReportByDishInfo()
                {
                    Id = g.FirstOrDefault().Id,
                    DishId = g.FirstOrDefault().DishId,
                    DishName = g.FirstOrDefault().DishName,
                }).ToList() ?? new List<ReportByDishInfo>();

                foreach (var item in group)
                {
                    ReportByDishInfo reportByDishInfo = new ReportByDishInfo();
                    List<ListReportByDish> listReportByDishes = new List<ListReportByDish>();
                    var lstReByDish = query.Where(x => x.DishId == item.DishId).ToList() ?? new List<ReportByDishInfo>();

                    var groupByRoom = lstReByDish.GroupBy(x => x.RoomId).Select(g => new ListReportByDish()
                    {
                        DishId = g.FirstOrDefault().DishId,
                        RoomId = g.FirstOrDefault().RoomId,
                        RoomName = g.FirstOrDefault().RoomName,
                    }).ToList() ?? new List<ListReportByDish>();

                    foreach (var g in groupByRoom)
                    {
                        ListReportByDish info = new ListReportByDish();
                        var lstRegisterDishByRoom = lstReByDish.Where(x => x.RoomId == g.RoomId).ToList() ?? new List<ReportByDishInfo>();
                        if (lstRegisterDishByRoom.Count > 0)
                        {
                            info.DishId = g.DishId;
                            info.RoomId = g.RoomId;
                            info.RoomName = g.RoomName;
                            info.NumberRegister = lstRegisterDishByRoom.Count;
                            listReportByDishes.Add(info);
                        }
                    }
                    reportByDishInfo.NumberTotal = lstReByDish.Count;
                    reportByDishInfo.DishId = item.DishId;
                    reportByDishInfo.DishName = item.DishName;
                    reportByDishInfo.lstData = listReportByDishes;
                    lst.Add(reportByDishInfo);
                }
            }

            return lst.OrderByDescending(x => x.NumberTotal).ToList() ?? new List<ReportByDishInfo>() ;
        }

        public List<User> GetLstUserByRoomId(int? roomId)
        {
            return db.Users.Where(x => x.IsDeleted == 0 && x.RoomID == roomId).ToList() ?? new List<User>();
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