using DKAC.Common;
using DKAC.Models.EntityModel;
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
            Employee employee = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            User user = (User)Session[CommonConstants.USER_SESSION];
            EmployeeRequestModel model = new EmployeeRequestModel();
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission(2, 1, currentUser);
            ViewBag.hasAddPermission = CheckPermission(2, 2, currentUser);
            ViewBag.hasUpdatePermission = CheckPermission(2, 4, currentUser);
            ViewBag.hasDeletePermission = CheckPermission(2, 8, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            model = _empRepo.GetListAllEmployee(employee, user, KeySearch, page, pageSize);
            return View(model);
        }

        [HttpGet]
        public ActionResult EditEmployeeInfo()
        {
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

            Employee em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            User u = (User)Session[CommonConstants.USER_SESSION];
            int? id = null;
            if (em != null) { id = em.id; }
            if (u != null) { id = u.id; }
            Employee emp = _empRepo.GetById(id);
            EmployeeInfo empInfo = new EmployeeInfo()
            {
                id = emp.id,
                FullName = emp.FullName,
                PassWord = emp.PassWord,
                UserName = emp.UserName,
                Birthday = emp.Birthday,
                Gender = emp.Gender,
                RoomID = emp.RoomID,
                Role = emp.Role,
                Email = emp.Email,
                CMND = emp.CMND,
                Tel = emp.Tel,
                Address = emp.Address,
            };
            return View("EditEmployeeInfo", empInfo);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Employee emp = _empRepo.GetById(id);
            EmployeeInfo empInfo = new EmployeeInfo()
            {
                id = emp.id,
                FullName = emp.FullName,
                PassWord = emp.PassWord,
                UserName = emp.UserName,
                Birthday = emp.Birthday,
                Gender = emp.Gender,
                RoomID = emp.RoomID,
                Role = emp.Role,
                Email = emp.Email,
                CMND = emp.CMND,
                Tel = emp.Tel,
                Address = emp.Address,
            };
            return View("Details", empInfo);
        }

        [HttpGet]
        public ActionResult Add()
        {
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
            Employee emp = _empRepo.GetById(id);
            EmployeeInfo empInfo = new EmployeeInfo()
            {
                id = emp.id,
                FullName = emp.FullName,
                PassWord = emp.PassWord,
                UserName = emp.UserName,
                Birthday = emp.Birthday,
                Gender = emp.Gender,
                RoomID = emp.RoomID,
                Role = emp.Role,
                Email = emp.Email,
                CMND = emp.CMND,
                Tel = emp.Tel,
                Address = emp.Address,

            };
            return View("Edit", empInfo);
        }

        [HttpPost]
        public ActionResult Add(Employee employee)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            employee.CreatedBy = user.id;
            employee.ModifyBy = user.id;
            var result = _empRepo.Add(employee);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Thêm thất bại" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditInfo(Employee employee)
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
        public ActionResult Edit(Employee employee)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            employee.ModifyBy = user.id;
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
            User user = (User)Session[CommonConstants.USER_SESSION];
            var emp = _empRepo.GetById(id);
            emp.ModifyBy = user.id;
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