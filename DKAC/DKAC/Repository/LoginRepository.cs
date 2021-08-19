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
    }
}