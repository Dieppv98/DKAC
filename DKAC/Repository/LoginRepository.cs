using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public class LoginRepository : ILoginRepository
    {
        DKACDbContext db = new DKACDbContext();
        NotificationComponent _nc = new NotificationComponent();

        public bool Login(string UserName, string Password)
        {
            var resultEm = db.Employees.Where(x => x.IsDeleted == 0 && x.UserName == UserName && x.PassWord == Password).FirstOrDefault();
            var resultU = db.Users.Where(x => x.IsDeleted == 0 && x.UserName == UserName && x.PassWord == Password).FirstOrDefault();
            if (resultEm != null || resultU != null) return true;
            return false;
        }

        public Employee GetEmployeeByUserName(string userName)
        {
            return db.Employees.Where(x => x.IsDeleted == 0 && x.UserName == userName).FirstOrDefault();
        }

        public User GetUserByUserName(string userName)
        {
            return db.Users.Where(x => x.IsDeleted == 0 && x.UserName == userName).FirstOrDefault();
        }

        public bool SignUp(EmployeeInfo model)
        {
            try
            {
                Employee em = new Employee();
                em.FullName = model.FullName;
                em.UserName = model.UserName;
                em.PassWord = model.PassWord;
                em.Address = model.Address;
                em.Birthday = model.Birthday;
                em.Gender = model.Gender;
                em.Tel = model.Tel;
                em.RoomID = model.RoomID;
                em.Role = 1;
                em.Email = model.Email;
                em.CreatedDate = DateTime.Now;
                em.ModifyDate = DateTime.Now;
                em.IsDeleted = 0;
                db.Employees.Add(em);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AccountModulPageInfo GetAccountModulPageInfo(int UserId)
        {
            var model = new AccountModulPageInfo();
            var rs = db.UserRoles.Where(x => x.UserId == UserId).ToList();
            var lstPerAction = new List<PermissionActionInfo>();
            var lstModul = new List<Modul>();
            var lstPage = new List<Page>();
            foreach (var item in rs)
            {
                var query = from pa in db.PermissionActions.Where(x => x.RoleId == item.RoleId && x.ActionKey != 0)
                            join p in db.Pages on pa.PageId equals p.id into lp
                            from lppa in lp.DefaultIfEmpty()
                            where lppa.IsDeleted != 1
                            select new PermissionActionInfo()
                            {
                                id = pa.id,
                                PageId = pa.PageId,
                                PageName = lppa.PageName,
                                RoleId = pa.RoleId,
                                ActionKey = pa.ActionKey,
                                ModulId = lppa.ModulId,
                                Url = lppa.Url,
                            };
                lstPerAction.AddRange(query.ToList());
            }
            foreach (var item in lstPerAction)
            {
                var modul = db.Moduls.FirstOrDefault(x => x.id == item.ModulId);
                var page = db.Pages.Where(x => x.id == item.PageId).FirstOrDefault();
                if (modul != null) lstModul.Add(modul);
                if (page != null) lstPage.Add(page);
            }
            model.ListPerAction = lstPerAction;
            model.ListModul = lstModul.Distinct().OrderBy(x => x.id).ToList();
            model.ListPage = lstPage.Distinct().ToList();
            return model;
        }

        public User GetUserByUserNameByCode(string userName, string code)
        {
            return db.Users.AsNoTracking().Where(x => x.IsDeleted == 0 && x.UserName.Contains(userName) && x.Code.Contains(code)).FirstOrDefault();
        }

        public void InsertOnlineUser(User user)
        {
            UsersOnline useronline = new UsersOnline()
            {
                UserId = user.id,
                UserName = user.UserName,
                FullName = user.FullName,
                TimeUpdate = DateTime.Now,
            };
            db.UsersOnlines.Add(useronline);
            db.SaveChanges();
        }

        public void RemoveOffUser(User user)
        {
            var userOff = db.UsersOnlines.FirstOrDefault(x => x.UserId == user.id) ?? new UsersOnline();
            db.UsersOnlines.Remove(userOff);
            db.SaveChanges();
        }
    }
}