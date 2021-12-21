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
    public class RoomController : BaseController
    {
        // GET: Room
        RoomRepository _roomRepo = new RoomRepository();
        public ActionResult Index(string KeySearch, int page = 1, int pageSize = 10)
        {
            Employee employee = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            User user = (User)Session[CommonConstants.USER_SESSION];
            RoomRequestModel model = new RoomRequestModel();
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission(3, 1, currentUser);
            ViewBag.hasAddPermission = CheckPermission(3, 2, currentUser);
            ViewBag.hasUpdatePermission = CheckPermission(3, 4, currentUser);
            ViewBag.hasDeletePermission = CheckPermission(3, 8, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            model = _roomRepo.GetListAllRoom(employee, user, KeySearch, page, pageSize);
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            RoomInfo info = new RoomInfo();
            var allMaster = _roomRepo.GetAllEmployeeByRoom(0);
            info.lstUser = allMaster.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.FullName.ToString() + "(" + a.UserName.ToString() + ")",
                    Value = a.id.ToString()
                };
            });
            return View(info);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var allEm = _roomRepo.GetAllEmployeeByRoom(id);
            List<SelectListItem> lstEm = allEm.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.FullName.ToString() + "(" + a.UserName.ToString() + ")",
                    Value = a.id.ToString()
                };
            });

            Room room = _roomRepo.GetById(id);
            RoomInfo roomInfo = new RoomInfo()
            {
                Id = room.id,
                RoomName = room.RoomName,
                RoomShortName = room.RoomShortName,
                Manager = room.Manager,
                Members = room.Members,
                DiaChi = room.DiaChi,
                SDT = room.SDT,
                lstUser = lstEm,
            };
            return View("Edit", roomInfo);
        }

        [HttpPost]
        public ActionResult Add(Room room)
        {
            User u = (User)Session[CommonConstants.USER_SESSION];
            room.CreatedBy = u.id;
            var result = _roomRepo.Add(room);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
            else if (result == -1)
            {
                return Json(new { status = -1, message = "Mã đã bị trùng" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Thêm thất bại" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Room room)
        {
            Employee em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            User u = (User)Session[CommonConstants.USER_SESSION];
            if (em != null) { room.ModifyBy = em.id; }
            if (u != null) { room.ModifyBy = u.id; }
            room.ModifyDate = DateTime.Now;
            var result = _roomRepo.Update(room);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            Employee em = (Employee)Session[CommonConstants.USER_SESSION];
            var emp = _roomRepo.GetById(id);
            emp.ModifyBy = em.id;
            var result = _roomRepo.Delete(id);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 1, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckDuplicatedShortName(string RoomShortName, int? id)
        {
            var name = _roomRepo.GetByShortName(RoomShortName, id);
            if (name == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckDuplicatedRoomName(string RoomName, int? id)
        {
            var name = _roomRepo.GetByRoomName(RoomName, id);
            if (name == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}
