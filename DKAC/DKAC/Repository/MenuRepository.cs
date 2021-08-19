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

        public int Delete(Menu menu)
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

        public MenuRequestModel GetAllMenu(string KeySearch, int page, int pageSize)
        {
            MenuRequestModel request = new MenuRequestModel();
            request.page = page;
            request.pageSize = pageSize;
            int startRow = (page - 1) * pageSize;
            var lst = from m in db.Menus.Where(x => x.IsDeleted == 0) select (m);

            if (!string.IsNullOrEmpty(KeySearch))
            {
                request.totalRecord = lst.Where(x => x.MenuCode.Contains(KeySearch) || x.MenuName.Contains(KeySearch)).ToList().Count;
                request.data = lst.Where(x => x.MenuCode.Contains(KeySearch) || x.MenuName.Contains(KeySearch)).OrderBy(x => x.ModifyDate).Skip(startRow).Take(pageSize).ToList();
            }
            else
            {
                request.totalRecord = lst.ToList().Count;
                request.data = lst.OrderBy(x => x.ModifyDate).Skip(startRow).Take(pageSize).ToList();
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
            request.KeyWord = KeySearch;
            return request;
        }

        public Menu GetByCodeId(string Code, int id)
        {
            return db.Menus.Where(x => x.IsDeleted == 0 && x.MenuCode == Code && x.id != id).FirstOrDefault();
        }

        public Menu GetById(int id)
        {
            Menu me = db.Menus.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault();
            if (me == null)
            {
                return new Menu();
            }
            return me;
        }

        public int Update(MenuInfo model)
        {
            try
            {
                Menu me = db.Menus.Where(x => x.IsDeleted == 0 && x.id == model.id).FirstOrDefault();
                me.MenuCode = model.MenuCode;
                me.MenuName = model.MenuName;
                me.Date = model.Date;
                me.Description = model.Description;
                me.ModifyDate = DateTime.Now;
                me.IsDeleted = 0;
                var details = db.MenuDetails.Where(x => x.IsDeleted == 0 && x.MenuId == me.id).ToList();
                foreach (var item in details)
                {
                    db.MenuDetails.Remove(item);
                }
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
    }
}