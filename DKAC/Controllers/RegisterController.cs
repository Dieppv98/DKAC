using DKAC.Common;
using DKAC.IRepository;
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
    public class RegisterController : BaseController
    {
        RegisterRepository _regRepo = new RegisterRepository();
        RoomRepository _roomRepo = new RoomRepository();
        EmployeeRepository _empRepo = new EmployeeRepository();
        IMenuRepository _menuRepo = new MenuRepository();

        // GET: Register
        [HttpGet]
        public ActionResult RegisterByPersonal(int? dishId)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyCaNhan, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }

            dishId = dishId == null ? 1 : dishId;
            RegisterByPersonalInfo model = new RegisterByPersonalInfo();
            model.Dish = new Dish();
            var em = (User)Session[CommonConstants.USER_SESSION];
            model.UserId = em.id;
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
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyCaNhan, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return null;
            }
            var em = (User)Session[CommonConstants.USER_SESSION];
            DateTime RegDate = DateTime.ParseExact(CurrentDate, "yyyy-MM-dd", null);
            var lstReg = _regRepo.GetAllRegister(em.id, RegDate);
            ViewBag.lstReg = lstReg;
            return PartialView();
        }

        [HttpPost]
        public ActionResult RegisterByPersonal(Register reg)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyCaNhan, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var em = (User)Session[CommonConstants.USER_SESSION];
            reg.UserId = em.id;
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
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyTapThe, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }

            List<RegisterByPersonalInfo> lst = new List<RegisterByPersonalInfo>();
            var lstEmInfo = _empRepo.GetEmInfo(currentUser.RoomID);
            foreach (var item in lstEmInfo)
            {
                RegisterByPersonalInfo reInfo = new RegisterByPersonalInfo();
                reInfo.UserId = item.id;
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

            var lstReg = _regRepo.GetRegisterByRoomId(currentUser.RoomID, DateTime.Now.Date);
            ViewBag.listReg = lstReg;

            return View(lst);
        }

        public ActionResult RegisterByMenu()
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyTapThe, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var userInfo = _empRepo.GetById(currentUser.id);
            var lstUserByRoomId = _empRepo.GetEmployees(userInfo.RoomID);
            RegisterByMenuInfo model = new RegisterByMenuInfo();
            var lstMenu = _regRepo.GetMenuByUserId(currentUser.id);
            model.lstMenu = lstMenu.ConvertAll(a => new SelectListItem()
            {
                Text = a.MenuName,
                Value = a.id.ToString(),
            }).ToList() ?? new List<SelectListItem>();
            model.RoomId = userInfo.RoomID;
            model.RoomName = userInfo.RoomName;
            model.UserId = userInfo.id;
            model.UserName = userInfo.UserName;
            model.lstUser = lstUserByRoomId;

            return View(model);
        }

        [HttpPost]
        public ActionResult RegisterByMenu(RegisterByMenuInfo model)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyTapThe, (int)Actions.Them, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var menuId = model.MenuId ?? 0;
            var menu = _menuRepo.GetById(menuId);
            if (menu.id <= 0)
            {
                return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
            }
            var ca = menu.Ca;
            var dateApply = model.DateApply;
            if (dateApply.Value.Date < DateTime.Now.Date || dateApply == null)
            {
                return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
            }
            if (dateApply.Value.Date == DateTime.Now.Date)
            {
                var hour = DateTime.Now.Hour;
                if (ca == 1)
                    if (hour > 9)
                        return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
                if (ca == 2)
                    if (hour > 15)
                        return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
                if (ca == 3)
                    if (hour > 22)
                        return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
            }
            model.MenuId = menu.id;
            model.Ca = ca;
            model.ModifyBy = currentUser.id;
            if(model.lstUserId == null)
            {
                model.lstUserId = new List<int>();
            }
            
            var result = _regRepo.RegisterByMenu(model);
            if (result > 0)
            {
                return Json(new { status = 1, message = "Đăng ký thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ListRegisterGroup(string CurrentDate)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyTapThe, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return null;
            }

            var em = (User)Session[CommonConstants.USER_SESSION];
            DateTime RegDate = DateTime.ParseExact(CurrentDate, "yyyy-MM-dd", null);
            var lstReg = _regRepo.GetRegisterByRoomId(em.RoomID, RegDate);
            ViewBag.lstReg = lstReg;
            return PartialView();
        }

        [HttpPost]
        public ActionResult RegisterByGroup(List<Register> model)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyTapThe, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }

            var result = _regRepo.RegisterByGroup(model);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Đăng ký thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Đăng ký thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            var hasDeletePermission = CheckPermission((int)PageId.DangKyTapThe, (int)Actions.Xoa, currentUser);
            if (!hasDeletePermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }

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

        public ActionResult GetMenuById(int id)
        {
            var menu = _menuRepo.GetById(id);

            return Json(menu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckDupplicateRegister(int MenuId, DateTime? DateApply, List<int> lstUserId)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.DangKyTapThe, (int)Actions.Xem, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }

            var data = _regRepo.CheckDupplicateRegister(MenuId, DateApply, currentUser.id, lstUserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}