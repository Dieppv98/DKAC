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
    public class DishController : BaseController
    {
        DishRepository _dishRepo = new DishRepository();
        SupplierRepository _supRepo = new SupplierRepository();
        HomeRepository _homeRepo = new HomeRepository();

        // GET: Dish
        public ActionResult Index(Dish KeySearch, int? FromCost, int? ToCost, int page = 1, int pageSize = 10)
        {
            DishRequestModel model = new DishRequestModel();
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission(5, 1, currentUser);
            ViewBag.hasAddPermission = CheckPermission(5, 2, currentUser);
            ViewBag.hasUpdatePermission = CheckPermission(5, 4, currentUser);
            ViewBag.hasDeletePermission = CheckPermission(5, 8, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            model = _dishRepo.GetAllDish(KeySearch, FromCost, ToCost, page, pageSize);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var lstSup = _supRepo.GetAll().Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.SupplierName.ToString(),
            }).ToList();
            ViewBag.lstSup = lstSup;

            var lstDishType = _homeRepo.GetAllDishTypes().Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.DishTypeName.ToString(),
            }).ToList();
            ViewBag.lstDishType = lstDishType;

            Dish dish = _dishRepo.GetById(id);
            Supplier sup = new Supplier();
            sup = _supRepo.GetById(dish.SupplierId);
            DishInfo dishInfo = new DishInfo()
            {
                id = dish.id,
                DishCode = dish.DishCode,
                DishName = dish.DishName,
                SupplierId = dish.SupplierId,
                SupplierCode = sup.SupplierCode,
                SupplierName = sup.SupplierName,
                DishTypeId = dish.DishTypeId,
                Cost = dish.Cost,
                Description = dish.Description,
                Image = dish.Image,
                JugmentPoint = dish.JugmentPoint,
                JugmentQty = dish.JugmentQty,
                RegisterQty = dish.RegisterQty,
            };
            return View("Edit", dishInfo);
        }

        public ActionResult Add(DishInfo model)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            model.CreatedBy = user.id;
            model.ModifyBy = user.id;
            model.DishCode = model.DishCode.ToUpper(); //Chuyển trường mã thành dạng chữ in hoa
            var result = _dishRepo.Add(model);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Thêm thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(DishInfo model)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            model.ModifyBy = user.id;
            model.DishCode = model.DishCode.ToUpper(); //Chuyển trường mã thành dạng chữ in hoa
            var result = _dishRepo.Update(model);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            var dish = _dishRepo.GetById(id);
            dish.ModifyBy = user.id;
            var result = _dishRepo.Delete(dish);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 1, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckDuplicatedCode(string DishCode, int id)
        {
            var result = _dishRepo.GetByCodeId(DishCode, id);
            if (result != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public string GetDishImageName(HttpPostedFileBase fileUpload)
        {
            fileUpload.SaveAs(Server.MapPath("~/Content/image/Dish/" + fileUpload.FileName));
            return fileUpload.FileName;
        }
    }
}