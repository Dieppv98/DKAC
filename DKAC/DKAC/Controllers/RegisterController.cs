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
    public class RegisterController : BaseController
    {
        RegisterRepository _regRepo = new RegisterRepository();
        RoomRepository _roomRepo = new RoomRepository();
        EmployeeRepository _empRepo = new EmployeeRepository();

        // GET: Register
        [HttpGet]
        public ActionResult RegisterByPersonal(int? dishId)
        {
            dishId = dishId == null ? 1 : dishId;
            RegisterByPersonalInfo model = new RegisterByPersonalInfo();
            model.Dish = new Dish();
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            model.EmployeeId = em.id;
            model.EmployeeName = em.FullName;
            var room = _regRepo.GetRoomById(em.RoomID);
            model.RoomId = room.id;
            model.RoomName = room.RoomName;
            if (dishId != null)
            {
                var dish = _regRepo.GetDishById(dishId);
                model.Dish = dish;
            }

            var lstDish = _regRepo.GetDishs();
            List<SelectListItem> listDish = lstDish.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.DishName.ToString() + "(" + a.DishTypeName + ")",
                    Value = a.id.ToString(),
                };
            });
            ViewBag.listDish = listDish;

            var lstReg = _regRepo.GetAllRegister(em.id, DateTime.Now.Date);
            ViewBag.listReg = lstReg;

            return View(model);
        }

        public PartialViewResult ListRegister(string CurrentDate)
        {
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            DateTime RegDate = DateTime.ParseExact(CurrentDate, "yyyy-MM-dd", null);
            var lstReg = _regRepo.GetAllRegister(em.id, RegDate);
            ViewBag.lstReg = lstReg;
            return PartialView();
        }

        [HttpPost]
        public ActionResult RegisterByPersonal(Register reg)
        {
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            reg.EmployeeId = em.id;
            reg.CreatedDate = DateTime.Now;
            reg.ModifyDate = DateTime.Now;
            var result = _regRepo.RegisterByPersonal(reg);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Đăng ký thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterByGroup()
        {
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            List<RegisterByPersonalInfo> lst = new List<RegisterByPersonalInfo>();
            var lstEmInfo = _empRepo.GetEmInfo(em.RoomID);
            foreach (var item in lstEmInfo)
            {
                RegisterByPersonalInfo reInfo = new RegisterByPersonalInfo();
                reInfo.EmployeeId = item.id;
                reInfo.EmployeeName = item.FullName;
                reInfo.RoomId = item.RoomID;
                reInfo.RoomName = item.RoomName;
                reInfo.Dish = _regRepo.GetDishById(1);
                lst.Add(reInfo);
            }

            var lstDish = _regRepo.GetDishs();
            List<SelectListItem> listDish = lstDish.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.DishName.ToString() + "(" + a.DishTypeName + ")",
                    Value = a.id.ToString(),
                };
            });
            ViewBag.listDish = listDish;

            var lstReg = _regRepo.GetRegisterByRoomId(em.RoomID, DateTime.Now.Date);
            ViewBag.listReg = lstReg;

            return View(lst);
        }

        public PartialViewResult ListRegisterGroup(string CurrentDate)
        {
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            DateTime RegDate = DateTime.ParseExact(CurrentDate, "yyyy-MM-dd", null);
            var lstReg = _regRepo.GetRegisterByRoomId(em.RoomID, RegDate);
            ViewBag.lstReg = lstReg;
            return PartialView();
        }

        [HttpPost]
        public ActionResult RegisterByGroup(List<Register> model)
        {
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            var result = _regRepo.RegisterByGroup(model);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Đăng ký thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            var result = _regRepo.DeleteReg(id);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Hủy đăng ký thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Hủy đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDish(int? id)
        {
            var dish = _regRepo.GetDishById(id);
            var result = new
            {
                id = dish.id,
                DishName = dish.DishName,
                Image = dish.Image,
                Cost = dish.Cost,
                CostFormat = @String.Format("{0:N0} VNĐ", dish.Cost),
                Jugment = dish.JugmentPoint + "(" + dish.JugmentQty + " đánh giá)", 
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}