using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public class MenuRepository : IMenuRepository
    {
        DKACDbContext db = new DKACDbContext();
        public int Add(MenuInfo model)
        {
            try
            {
                Menu me = new Menu()
                {
                    MenuCode = model.MenuCode,
                    MenuName = model.MenuName,
                    CreatedDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    Date = model.Date,
                    Description = model.Description,
                };
                db.Menus.Add(me);
                db.SaveChanges();
                foreach (var item in model.details)
                {
                    item.MenuId = me.id;
                    db.MenuDetails.Add(item);
                }
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Delete(MenuInfo menu)
        {
            var m = db.Menus.FirstOrDefault(x => x.IsDeleted == 0 && x.id == menu.id);
            if (m != null)
            {
                m.IsDeleted = 1;
                m.ModifyDate = DateTime.Now;
                var details = db.MenuDetails.Where(x => x.IsDeleted == 0 && x.MenuId == m.id).ToList();
                foreach (var item in details)
                {
                    item.IsDeleted = 1;
                    item.ModifyDate = DateTime.Now;
                }
                db.SaveChanges();
                return 1;
            }
            return 0;
        }

        public MenuRequestModel GetAllMenu(MenuRequestModel request, int pageIndex, int recordPerPage, out int totalCount, User user)
        {
            pageIndex = pageIndex - 1;
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                request.KeyWord = request.KeyWord.Trim();
            }
            int? userId = null;
            if (user.UserGroupId != (int)GroupUser.admin)
            {
                userId = user.id;
            }

            var query = from m in db.Menus
                        where (!userId.HasValue || m.CreatedBy == userId) && (string.IsNullOrEmpty(request.KeyWord) || m.MenuName.Contains(request.KeyWord) || m.MenuCode.Contains(request.KeyWord))
                        select m;
            totalCount = query.Count();
            var rs = query.OrderByDescending(x => x.Date).Skip(pageIndex * recordPerPage).Take(recordPerPage).ToList() ?? new List<Menu>();
            request.data = rs;
            return request ?? new MenuRequestModel();
        }

        public Menu GetByCodeId(string Code, int id)
        {
            return db.Menus.Where(x => x.IsDeleted == 0 && x.MenuCode == Code && x.id != id).FirstOrDefault();
        }

        public MenuInfo GetById(int id)
        {
            MenuInfo menuInfo = new MenuInfo();
            Menu me = db.Menus.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault() ?? new Menu();

            var dataDetail = (from m in db.MenuDetails
                              where m.MenuId == me.id
                              join d in db.Dishes on m.DishId equals d.id into ld
                              from ldm in ld.DefaultIfEmpty()
                              select new MenuDetailInfo()
                              {
                                  id = m.id,
                                  DishId = m.DishId,
                                  IndexDate = m.IndexDate,
                                  MenuId = m.MenuId,
                                  ImageDishUrl = ldm.Image,
                                  DishName = ldm.DishName,
                                  DishCode = ldm.DishCode,
                              }).ToList() ?? new List<MenuDetailInfo>();
            menuInfo.id = me.id;
            menuInfo.DishId1 = dataDetail.FirstOrDefault(x => x.IndexDate == 1).DishId ?? 0;
            menuInfo.DishId2 = dataDetail.FirstOrDefault(x => x.IndexDate == 2).DishId ?? 0;
            menuInfo.DishId3 = dataDetail.FirstOrDefault(x => x.IndexDate == 3).DishId ?? 0;
            menuInfo.DishId4 = dataDetail.FirstOrDefault(x => x.IndexDate == 4).DishId ?? 0;
            menuInfo.DishId5 = dataDetail.FirstOrDefault(x => x.IndexDate == 5).DishId ?? 0;
            menuInfo.DishId6 = dataDetail.FirstOrDefault(x => x.IndexDate == 6).DishId ?? 0;
            menuInfo.DishId7 = dataDetail.FirstOrDefault(x => x.IndexDate == 7).DishId ?? 0;
            menuInfo.MenuCode = me.MenuCode;
            menuInfo.MenuName = me.MenuName;
            menuInfo.Ca = me.Ca;
            menuInfo.Date = me.Date;
            menuInfo.ModifyDate = me.ModifyDate;
            menuInfo.Description = me.Description;
            menuInfo.detailInfos = dataDetail;
            return menuInfo;
        }

        public int Update(MenuInfo model)
        {
            try
            {
                Menu me = db.Menus.Where(x => x.IsDeleted == 0 && x.id == model.id).FirstOrDefault() ?? new Menu();
                me.MenuCode = model.MenuCode;
                me.MenuName = model.MenuName;
                me.Description = model.Description;
                me.Ca = model.Ca;
                me.IsDeleted = 0;

                if (me.id <= 0)
                {
                    me.CreatedBy = model.CreatedBy;
                    me.Date = DateTime.Now;
                    db.Menus.Add(me);
                    db.SaveChanges();
                }

                var details = db.MenuDetails.Where(x => x.IsDeleted != 1 && x.MenuId == me.id).ToList();
                foreach (var item in details)
                {
                    db.MenuDetails.Remove(item);
                }
                foreach (var item in model.details)
                {
                    item.MenuId = me.id;
                    db.MenuDetails.Add(item);
                }

                me.ModifyDate = DateTime.Now;
                me.ModifyBy = model.ModifyBy;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<Dish> GetAllDish()
        {
            return db.Dishes.Where(x => x.IsDeleted != 1).ToList();
        }

        public Menu GetByMenuCode(string MenuCode, int? id)
        {
            var data = db.Menus.Where(x => x.IsDeleted == 0 && x.id != id && x.MenuCode == MenuCode).FirstOrDefault();
            return data;
        }

    }
}