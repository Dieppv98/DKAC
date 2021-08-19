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
using System.Runtime.Caching;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DKAC.Models.Enum;

namespace DKAC.Controllers
{
    public class SupplierController : BaseController
    {
        public IMemoryCache _cache;

        SupplierRepository _supRepo = new SupplierRepository();

        // GET: Supplier
        public ActionResult Index(string KeySearch, int page = 1, int pageSize = 10)
        {
            SupplierRequestModel model = new SupplierRequestModel();
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            model = GetAllSupplierByCache(KeySearch, page, pageSize);
            ViewBag.hasViewPermission = CheckPermission(1, 1, currentUser);
            ViewBag.hasAddPermission = CheckPermission(1, 2, currentUser);
            ViewBag.hasUpdatePermission = CheckPermission(1, 4, currentUser);
            ViewBag.hasDeletePermission = CheckPermission(1, 8, currentUser);
            model.hasDataPermission = currentUser.id;
            if (!ViewBag.hasViewPermission)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            return View(model);
        }


        public SupplierRequestModel GetAllSupplierByCache(string KeySearch, int page, int pageSize)
        {
            int a;
            SupplierRequestModel result = new SupplierRequestModel();
            var cache = System.Runtime.Caching.MemoryCache.Default;
            if (cache.Get(KeySearch + page + "MemoryCache_GetAllSupplier") != null)
            {
                var cacheGetAllSup = _supRepo.UpdateCacheGetAllSupplier(a = 0);

                result = (SupplierRequestModel)cache.Get(KeySearch + page + "MemoryCache_GetAllSupplier");
                return result;
            }
            else
            {
                var cacheGetAllSup = _supRepo.UpdateCacheGetAllSupplier(a = 1);

                if (cacheGetAllSup == 2)
                {
                    var cachePolicty = new CacheItemPolicy();
                    cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(1);
                    result = _supRepo.GetAllSupplier(KeySearch, page, pageSize);
                    cache.Add(KeySearch + page + "MemoryCache_GetAllSupplier", result, cachePolicty);
                    return result;
                }
            }
            return _supRepo.GetAllSupplier(KeySearch, page, pageSize);
        }


        [HttpPost]
        public ActionResult ClearCachingSupplier()
        {
            try
            {
                string cache = "MemoryCache_GetAllSupplier";
                RemoveCache(cache);
                return Json(new { status = 1, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = 0, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult ClearCachingSupplierById(int id)
        {
            try
            {
                RemoveCache(id + "MemoryCache_GetSupplierById");
                return Json(new { status = 1, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = 0, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }


        //Using MemoryCache
        public SupplierInfo GetCacheById(int id)
        {
            var currentUser = (User)Session[CommonConstants.USER_SESSION];
            SupplierInfo result = new SupplierInfo();
            try
            {
                var cache = System.Runtime.Caching.MemoryCache.Default;
                if (cache.Get(id + "MemoryCache_GetSupplierById") == null)
                {
                    var cachePolicty = new CacheItemPolicy();
                    cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(24);
                    using (var db = new DKACDbContext())
                    {
                        var sup = _supRepo.GetById(id, currentUser.id);
                        result.id = sup.id;
                        result.SupplierCode = sup.SupplierCode;
                        result.SupplierName = sup.SupplierName;
                        result.Address = sup.Address;
                        result.Tel = sup.Tel;
                        cache.Add(id + "MemoryCache_GetSupplierById", result, cachePolicty);
                    }
                }
                else
                {
                    result = (SupplierInfo)cache.Get(id + "MemoryCache_GetSupplierById");
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var s = GetCacheById(id);
                return View("Edit", s);
            }
            catch
            {
                var currentUser = (User)Session[CommonConstants.USER_SESSION];
                Supplier sup = _supRepo.GetById(id, currentUser.id);
                //if (currentUser.UserName != "admin")
                //{
                //    if (sup == null) return RedirectToAction("NotPermission", "Home");
                //}
                SupplierInfo supInfo = new SupplierInfo()
                {
                    id = sup.id,
                    SupplierCode = sup.SupplierCode,
                    SupplierName = sup.SupplierName,
                    Address = sup.Address,
                    Tel = sup.Tel,
                };
                return View("Edit", supInfo);
            }
        }


        public ActionResult Add(SupplierInfo model)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            model.CreatedBy = user.id;
            model.ModifyBy = user.id;
            model.SupplierCode = model.SupplierCode.ToUpper(); //Chuyển trường mã thành dạng chữ in hoa
            var result = _supRepo.Add(model);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Thêm thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(SupplierInfo model)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            model.ModifyBy = user.id;
            model.SupplierCode = model.SupplierCode.ToUpper(); //Chuyển trường mã thành dạng chữ in hoa
            var result = _supRepo.Update(model);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 0, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            User user = (User)Session[CommonConstants.USER_SESSION];
            var sup = _supRepo.GetById(id, user.id);
            sup.ModifyBy = user.id;
            var result = _supRepo.Delete(sup);
            if (result == 1)
            {
                return Json(new { status = 1, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 1, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckDuplicatedCode(string SupplierCode, int id)
        {
            var result = _supRepo.GetByCodeId(SupplierCode, id);
            if (result != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}