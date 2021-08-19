using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using DKAC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using DKAC.Models.Enum;

namespace DKAC.Controllers
{
    public class DishController : BaseController
    {
        DishRepository _dishRepo = new DishRepository();
        SupplierRepository _supRepo = new SupplierRepository();
        HomeRepository _homeRepo = new HomeRepository();
        NotificationComponent _nc = new NotificationComponent();

        //[OutputCache(Duration = 3600)]
        public ActionResult Index(Dish KeySearch, int? FromCost, int? ToCost, int page = 1, int pageSize = 10)
        {
            DishRequestModel model = new DishRequestModel();
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasViewPermission = CheckPermission((int)PageId.QuanLySuatAn, (int)Actions.Xem, currentUser);
            ViewBag.hasAddPermission = CheckPermission((int)PageId.QuanLySuatAn, (int)Actions.Them, currentUser);
            ViewBag.hasUpdatePermission = CheckPermission((int)PageId.QuanLySuatAn, (int)Actions.Sua, currentUser);
            ViewBag.hasDeletePermission = CheckPermission((int)PageId.QuanLySuatAn, (int)Actions.Xoa, currentUser);
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            model = GetAllDishByCache(KeySearch, FromCost, ToCost, page, pageSize);
            model.hasDataPermission = currentUser.id;
            return View(model);
        }


        public DishRequestModel GetAllDishByCache(Dish KeySearch, int? FromCost, int? ToCost, int page, int pageSize)
        {
            int a;
            DishRequestModel result = new DishRequestModel();
            var cache = System.Runtime.Caching.MemoryCache.Default;
            if (cache.Get(KeySearch + $"{FromCost}{ToCost}{page}" + "MemoryCache_GetAllDish") != null)
            {
                var cacheGetAllDish = _dishRepo.UpdateCacheGetAllDish(a = 0);

                result = (DishRequestModel)cache.Get(KeySearch + $"{FromCost}{ToCost}{page}" + "MemoryCache_GetAllDish");
                return result;

            }
            else
            {
                var cacheGetAllDish = _dishRepo.UpdateCacheGetAllDish(a = 1);

                if (cacheGetAllDish == 2)
                {
                    var cachePolicty = new CacheItemPolicy();
                    cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(1);
                    result = _dishRepo.GetAllDish(KeySearch, FromCost, ToCost, page, pageSize);
                    cache.Add(KeySearch + $"{FromCost}{ToCost}{page}" + "MemoryCache_GetAllDish", result, cachePolicty);
                    return result;
                }
            }
            return _dishRepo.GetAllDish(KeySearch, FromCost, ToCost, page, pageSize);
        }


        [HttpPost]
        public ActionResult ClearCachingDish()
        {
            try
            {
                string cache = "MemoryCache_GetAllDish";
                RemoveCache(cache);
                return Json(new { status = 1, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = 0, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }


        //[ChildActionOnly]
        //[OutputCache(CacheProfile = "Dish1Hours")]
        public ActionResult Edit(int id)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasUpdatePermission = CheckPermission((int)PageId.QuanLySuatAn, (int)Actions.Sua, currentUser);
            if (!ViewBag.hasUpdatePermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }

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
            var db = new DKACDbContext();
            var sup = db.Suppliers.Where(x => x.id == dish.SupplierId).FirstOrDefault();
            if (sup == null)
                sup = new Supplier();
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
            ViewBag.hasPermission = CheckPermission((int)PageId.QuanLySuatAn, (int)Actions.Them, user);
            if (!ViewBag.hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            model.CreatedBy = user.id;
            model.ModifyBy = user.id;
            model.DishCode = model.DishCode.ToUpper(); //Chuyển trường mã thành dạng chữ in hoa
            var result = _dishRepo.Add(model);
            if (result > 0)
            {
                TriggerNotiAdDish(result);

                return Json(new { status = 1, message = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Thêm thất bại" }, JsonRequestBehavior.AllowGet);
        }

        private void TriggerNotiAdDish(int id)
        {
            _nc.NotifyAddNewDish(id);
        }

        public ActionResult Update(DishInfo model)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.hasPermission = CheckPermission((int)PageId.QuanLySuatAn, (int)Actions.Sua, user);
            if (!ViewBag.hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
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
            ViewBag.hasPermission = CheckPermission((int)PageId.QuanLySuatAn, (int)Actions.Xoa, user);
            if (!ViewBag.hasPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }

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