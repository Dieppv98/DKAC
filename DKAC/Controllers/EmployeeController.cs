using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using DKAC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Controllers
{
    public class EmployeeController : BaseController
    {
        EmployeeRepository _empRepo = new EmployeeRepository();
        // GET: Employee
        public ActionResult Index(string KeySearch, int page = 1, int pageSize = 20)
        {
            UserRequestModel model = new UserRequestModel();
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Xem, currentUser);
            ViewBag.hasAddPermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Them, currentUser);
            ViewBag.hasUpdatePermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Sua, currentUser);
            ViewBag.hasDeletePermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Xoa, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            model = _empRepo.GetListAllEmployee(currentUser, KeySearch, page, pageSize);
            return View(model);
        }

        //[HttpGet]
        //public ActionResult EditEmployeeInfo()
        //{
        //    var allRoom = _empRepo.GetAllRoom();
        //    List<SelectListItem> lstAllRoom = allRoom.ConvertAll(a =>
        //    {
        //        return new SelectListItem()
        //        {
        //            Text = a.RoomName.ToString(),
        //            Value = a.id.ToString()
        //        };
        //    });
        //    ViewBag.All = lstAllRoom;

        //    Employee em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
        //    User u = (User)Session[CommonConstants.USER_SESSION];
        //    int? id = null;
        //    if (em != null) { id = em.id; }
        //    if (u != null) { id = u.id; }
        //    Employee emp = _empRepo.GetById(id);
        //    EmployeeInfo empInfo = new EmployeeInfo()
        //    {
        //        id = emp.id,
        //        FullName = emp.FullName,
        //        PassWord = emp.PassWord,
        //        UserName = emp.UserName,
        //        Birthday = emp.Birthday,
        //        Gender = emp.Gender,
        //        RoomID = emp.RoomID,
        //        Role = emp.Role,
        //        Email = emp.Email,
        //        CMND = emp.CMND,
        //        Tel = emp.Tel,
        //        Address = emp.Address,
        //    };
        //    return View("EditEmployeeInfo", empInfo);
        //}

        [HttpGet]
        public ActionResult Details(int id)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            var hasPermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Xem, currentUser);
            if (!hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            UserInfo emp = _empRepo.GetById(id);
            return View("Details", emp);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            var hasPermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Them, currentUser);
            if (!hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var allRoom = _empRepo.GetAllRoom();
            List<SelectListItem> lstAllRoom = allRoom.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.RoomName.ToString(),
                    Value = a.id.ToString(),
                };
            });
            ViewBag.All = lstAllRoom;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            var hasPermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Sua, currentUser);
            if (!hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var allRoom = _empRepo.GetAllRoom();
            List<SelectListItem> lstAllRoom = allRoom.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.RoomName.ToString(),
                    Value = a.id.ToString()
                };
            });
            ViewBag.All = lstAllRoom;
            UserInfo emp = _empRepo.GetById(id);
            return View("Edit", emp);
        }

        [HttpPost]
        public ActionResult Add(User employee)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            var hasPermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Them, currentUser);
            if (!hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            employee.CreatedBy = currentUser.id;
            employee.ModifyBy = currentUser.id;
            employee.PassWord = Encryption.EncryptPassword(employee.PassWord);
            var result = _empRepo.Add(employee);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Thêm thất bại" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditInfo(UserInfo employee)
        {
            Employee em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            employee.ModifyBy = em.id;
            employee.ModifyDate = DateTime.Now;
            var result = _empRepo.Update(employee);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(UserInfo employee)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            var hasPermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Sua, currentUser);
            if (!hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            employee.ModifyBy = currentUser.id;
            employee.ModifyDate = DateTime.Now;
            var result = _empRepo.Update(employee);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            var hasPermission = CheckPermission((int)PageId.QuanLyNguoiDung, (int)Actions.Xoa, currentUser);
            if (!hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            User user = (User)Session[CommonConstants.USER_SESSION];
            var result = _empRepo.Delete(id);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 1, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckDuplicatedUserName(string UserName, int? id)
        {
            var name = _empRepo.GetByUserName(UserName, id);
            if (name == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

    }
}