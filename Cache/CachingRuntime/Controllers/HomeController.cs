using CachingRuntime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace CachingRuntime.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var Appalias = this.AppID;
            var data=  GetCategories(Appalias);
            return View(data);
        }

        public ActionResult ClearCaching()
        {

            if (clearALlcaching() == true)
            {
                ViewBag.Result = "Xóa cached thành công!!";
            }
            else
            {
                ViewBag.Result = "Lỗi! Xóa cached !!";

            }

            return View();
        }


        public List<Category> GetCategories(string AppAlias)
        {
            List<Category> result = new List<Category>();
            try
            {
                var cache = MemoryCache.Default;
             

                if (cache.Get(AppAlias + "MemoryCache_GetCategories") == null)
                {
                    var cachePolicty = new CacheItemPolicy();
                    cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(200);
                  

                    using (var db = new NorthwindEntities())
                    {
                        result = db.Categories.ToList();

                        cache.Add(AppAlias + "MemoryCache_GetCategories", result, cachePolicty);
                    }
                }
                else
                {
                    result = (List<Category>)cache.Get(AppAlias + "MemoryCache_GetCategories");
                }


            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }



        public void Remove(string cacheKey)
        {
            var lstCaches = MemoryCache.Default.Where(x => x.Key.ToLower().Contains(cacheKey.ToLower())).ToList();
            for (int i = 0; i < lstCaches.Count; i++)
                MemoryCache.Default.Remove(lstCaches[i].Key);
        }

        public bool clearALlcaching()
        {
            bool result = false;

            var Appalias = this.AppID;


            try
            {
                Remove(Appalias + "MemoryCache_GetCategories");
            

                result = true;
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }



    }
}