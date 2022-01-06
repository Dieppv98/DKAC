using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        DKACDbContext db = new DKACDbContext();

        public List<DishInfo> GetDishs()
        {
            var query = from d in db.Dishes
                        where d.IsDeleted == 0
                        join dt in db.DishTypes on d.DishTypeId equals dt.id into ldt
                        from ld in ldt.DefaultIfEmpty()
                        select new DishInfo()
                        {
                            id = d.id,
                            Cost = d.Cost,
                            Description = d.Description,
                            DishCode = d.DishCode,
                            DishName = d.DishName,
                            DishTypeId = d.DishTypeId,
                            DishTypeName = ld.DishTypeName,
                            Image = d.Image,
                            JugmentPoint = d.JugmentPoint,
                            JugmentQty = d.JugmentQty,
                            RegisterQty = d.RegisterQty,
                            SupplierId = d.SupplierId,
                        };
            return query.ToList();
        }

        public int RegisterByPersonal(Register model)
        {
            try
            {
                var reg = db.Registers.FirstOrDefault(x => x.UserId == model.UserId && x.RegisterDate == model.RegisterDate && x.Ca == model.Ca && x.DishId == model.DishId);
                if (reg != null)
                {
                    reg.Quantity += model.Quantity;
                }
                else
                {
                    db.Registers.Add(model);
                }
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int RegisterByGroup(List<Register> model)
        {
            try
            {
                foreach (var item in model)
                {
                    if (item.DishId != null && item.Ca != null)
                    {
                        var reg = db.Registers.FirstOrDefault(x => x.UserId == item.UserId && x.RegisterDate == item.RegisterDate && x.Ca == item.Ca && x.DishId == item.DishId);
                        if (reg != null)
                        {
                            reg.Quantity += item.Quantity;
                            reg.ModifyDate = DateTime.Now;
                        }
                        else
                        {
                            item.CreatedDate = DateTime.Now;
                            item.ModifyDate = DateTime.Now;
                            db.Registers.Add(item);
                        }
                    }
                }
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<RegisterByPersonalInfo> GetAllRegister(int emId, DateTime regDate)
        {
            var query = from r in db.Registers
                        where r.UserId == emId && r.RegisterDate == regDate
                        join e in db.Employees on r.UserId equals e.id into le
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

        public Room GetRoomById(int? id)
        {
            return db.Rooms.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault();
        }

        public Dish GetDishById(int? id)
        {
            return db.Dishes.FirstOrDefault(x => x.IsDeleted == 0 && x.id == id);
        }

        public int DeleteReg(int? id)
        {
            if (id != null)
            {
                var reg = db.Registers.FirstOrDefault(x => x.id == id);
                db.Registers.Remove(reg);
                db.SaveChanges();
                return 1;
            }
            return 0;
        }

        public List<RegisterByPersonalInfo> GetRegisterByRoomId(int? roomId, DateTime regDate)
        {
            var query = from r in db.Registers
                        join e in db.Users on r.UserId equals e.id into le
                        from lr1 in le.DefaultIfEmpty()
                        join d in db.Dishes on r.DishId equals d.id into ld
                        from lr2 in ld.DefaultIfEmpty()
                        join ro in db.Rooms on lr1.RoomID equals ro.id into lro
                        from lr3 in lro.DefaultIfEmpty()
                        where lr3.id == roomId && r.RegisterDate == regDate
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

        public List<Menu> GetMenuByUserId(int id)
        {
            var data = db.Menus.Where(x => x.IsDeleted != 1 && x.CreatedBy == id).OrderByDescending(x => x.Date).ToList() ?? new List<Menu>();
            return data;
        }

        public List<RegisterByMenuInfo> CheckDupplicateRegister(int MenuId, DateTime? DateApply, int userId, List<int> lstUserId)
        {
            var user = db.Users.FirstOrDefault(x => x.id == userId) ?? new User();
            var menu = db.Menus.FirstOrDefault(x => x.id == MenuId) ?? new Menu();
            var dateEnd = DateApply.Value.AddDays(7);
            var ca = menu.Ca;
            if (lstUserId == null)
                lstUserId = new List<int>();
            var lstReg = (from u in db.Users where !lstUserId.Contains(u.id)
                          where u.RoomID == user.RoomID
                          join r in db.Registers on u.id equals r.UserId
                          join d in db.Dishes on r.DishId equals d.id
                          where r.Ca == ca && r.RegisterDate >= DateApply && r.RegisterDate <= dateEnd
                          select new RegisterByMenuInfo()
                          {
                              RegisterId = r.id,
                              Ca = r.Ca,
                              RegDate = r.RegisterDate,
                              DishId = r.DishId,
                              DishName = d.DishName,
                              Quantity = r.Quantity,
                              UserId = u.id,
                              UserName = u.FullName + "(" + u.UserName + ")",
                          }).OrderBy(x => x.UserId).ToList() ?? new List<RegisterByMenuInfo>();

            return lstReg;
        }

        public int RegisterByMenu(RegisterByMenuInfo model)
        {
            try
            {
                var menuId = model.MenuId ?? 0;
                var lstDupClinet = model.lstDupplicate ?? new List<RegisterByDuplplicate>();
                var user = db.Users.FirstOrDefault(x => x.id == model.ModifyBy) ?? new User();
                var lstUserByRoom = db.Users.Where(x => x.RoomID == user.RoomID && !model.lstUserId.Contains(x.id)).ToList() ?? new List<User>();
                var menuDetails = db.MenuDetails.Where(x => x.MenuId == menuId).ToList() ?? new List<MenuDetail>();
                var lstDupplicate = CheckDupplicateRegister(menuId, model.DateApply.Value.Date, user.id, model.lstUserId) ?? new List<RegisterByMenuInfo>();

                foreach (var item in lstUserByRoom)
                {
                    var dateApply = model.DateApply.Value.Date;
                    var dateEnd = dateApply.AddDays(7);
                    var regDup = lstDupplicate.Where(x => x.UserId == item.id).ToList() ?? new List<RegisterByMenuInfo>();
                    //var dateDup = regDup.RegDate;
                    int index = 0;
                    while (dateApply <= dateEnd)
                    {
                        index += 1;
                        var detail = menuDetails.FirstOrDefault(x => x.IndexDate == index);
                        if (detail != null)
                        {
                            var regDupByDate = regDup.Where(x => x.RegDate == dateApply && x.UserId == item.id).ToList();
                            if (regDupByDate != null && regDupByDate.Count > 0)
                            {
                                foreach (var i in regDupByDate)
                                {
                                    var element = lstDupClinet.FirstOrDefault(x => x.RegisterId == i.RegisterId);
                                    if (element != null)
                                    {
                                        if (element.Skip != true)
                                        {
                                            var regUpdate = db.Registers.FirstOrDefault(x => x.id == element.RegisterId) ?? new Register();
                                            // thay thế đăng ký trước đó
                                            if (element.Replace == true)
                                            {
                                                regUpdate.DishId = detail.DishId;
                                                regUpdate.ModifyDate = DateTime.Now;
                                                regUpdate.ModifyBy = model.ModifyBy;
                                            }
                                            // add thêm 
                                            if (element.Plus == true)
                                            {
                                                var regNew = new Register()
                                                {
                                                    Ca = regUpdate.Ca,
                                                    Quantity = 1,
                                                    UserId = regUpdate.UserId,
                                                    RegisterDate = regUpdate.RegisterDate,
                                                    CreatedDate = DateTime.Now,
                                                    ModifyDate = DateTime.Now,
                                                    CreatedBy = model.ModifyBy,
                                                    DishId = detail.DishId,
                                                };
                                                db.Registers.Add(regNew);
                                            }
                                            //xóa đăng ký trước đó
                                            if (element.Remove == true)
                                            {
                                                db.Registers.Remove(regUpdate);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var newReg = new Register()
                                {
                                    Ca = model.Ca,
                                    DishId = detail.DishId,
                                    CreatedBy = model.ModifyBy,
                                    CreatedDate = DateTime.Now,
                                    Quantity = 1,
                                    UserId = item.id,
                                    RegisterDate = dateApply
                                };
                                db.Registers.Add(newReg);
                            }
                        }
                        dateApply = dateApply.AddDays(1);
                    }
                }

                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}