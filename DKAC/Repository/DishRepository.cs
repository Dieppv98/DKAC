using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public class DishRepository : IDishRepository
    {
        DKACDbContext db = new DKACDbContext();

        public int Add(DishInfo model)
        {
            try
            {
                Dish dish = new Dish();
                dish.DishCode = model.DishCode;
                dish.DishName = model.DishName;
                dish.DishTypeId = model.DishTypeId;
                dish.SupplierId = model.SupplierId;
                dish.Cost = model.Cost;
                dish.Description = model.Description;
                dish.JugmentPoint = 5;
                dish.JugmentQty = 0;
                dish.RegisterQty = 0;
                dish.Image = model.Image;
                dish.CreatedDate = DateTime.Now;
                dish.ModifyDate = DateTime.Now;
                dish.IsDeleted = 0;
                db.Dishes.Add(dish);
                db.SaveChanges();
                return dish.id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Delete(Dish dish)
        {
            var info = db.Dishes.Where(x => x.IsDeleted == 0 && x.id == dish.id).FirstOrDefault();
            if (info != null)
            {
                info.IsDeleted = 1;
                info.ModifyDate = DateTime.Now;
                db.SaveChanges();
                return 1;
            }
            return 0;
        }

        public DishRequestModel GetAllDish(Dish KeySearch, int? FromCost, int? ToCost, int page, int pageSize)
        {
            DishRequestModel request = new DishRequestModel();
            var query = from d in db.Dishes
                        where d.IsDeleted == 0
                        select (d);

            request.page = page;
            request.pageSize = pageSize;

            int startRow = (page - 1) * pageSize;
            if (!string.IsNullOrEmpty(KeySearch.DishName))
            {
                query.Where(x => x.DishCode.Contains(KeySearch.DishName) || x.DishName.Contains(KeySearch.DishName));
            }
            if (KeySearch.SupplierId != null)
            {
                query.Where(x => x.SupplierId == KeySearch.SupplierId);
            }
            if (FromCost != null)
            {
                query.Where(x => x.Cost >= FromCost);
            }
            if (ToCost != null)
            {
                query.Where(x => x.Cost <= ToCost);
            }

            request.totalRecord = query.ToList().Count;
            var joinQuery = from d in query
                            join s in db.Suppliers.Where(x => x.IsDeleted == 0) on d.SupplierId equals s.id into ls
                            from ld in ls.DefaultIfEmpty()
                            select new DishInfo()
                            {
                                id = d.id,
                                DishCode = d.DishCode,
                                DishName = d.DishName,
                                SupplierId = d.SupplierId,
                                DishTypeId = d.DishTypeId,
                                SupplierCode = ld.SupplierCode,
                                SupplierName = ld.SupplierName,
                                Cost = d.Cost,
                                Description = d.Description,
                                Image = d.Image,
                                JugmentPoint = d.JugmentPoint,
                                JugmentQty = d.JugmentQty,
                                RegisterQty = d.RegisterQty,
                                CreatedBy = d.CreatedBy,
                            };
            request.data = joinQuery.ToList();
            int totalPage = 0;
            if (request.totalRecord % pageSize == 0)
            {
                totalPage = request.totalRecord / pageSize;
            }
            else
            {
                totalPage = request.totalRecord / pageSize + 1;
            }
            request.totalPage = totalPage;
            request.KeySearch = KeySearch;
            return request;
        }

        public Dish GetByCodeId(string Code, int id)
        {
            return db.Dishes.Where(x => x.IsDeleted == 0 && x.DishCode == Code && x.id != id).FirstOrDefault();
        }

        public Dish GetById(int id)
        {
            Dish dish = db.Dishes.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault();
            if (dish == null)
            {
                return new Dish();
            }
            return dish;
        }

        public int Update(DishInfo model)
        {
            try
            {
                Dish dish = db.Dishes.Where(x => x.IsDeleted == 0 && x.id == model.id).FirstOrDefault();
                dish.DishCode = model.DishCode;
                dish.DishName = model.DishName;
                dish.SupplierId = model.SupplierId;
                dish.DishTypeId = model.DishTypeId;
                dish.Cost = model.Cost;
                dish.Description = model.Description;
                if (model.Image != null)
                {
                    dish.Image = model.Image;
                }
                dish.ModifyDate = DateTime.Now;
                dish.IsDeleted = 0;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int UpdateCacheGetAllDish(int a)
        {
            var logtest = db.LogTimeCaches.Where(x => x.CacheType == (int)CacheType.MemoryCache_GetAllDish).ToList();
            var log = db.LogTimeCaches.Where(x => x.CacheType == (int)CacheType.MemoryCache_GetAllDish).ToList().OrderByDescending(x => x.id);
            // add bản ghi đầu tiên
            if (logtest == null || logtest.Count() == 0)
            {
                LogTimeCache newLogTime = new LogTimeCache()
                {
                    CreatedDate = DateTime.Now,
                    PreOder = 1,
                    CacheType = (int)CacheType.MemoryCache_GetAllDish,
                    Description = "Cache All Dish",
                };
                db.LogTimeCaches.Add(newLogTime);
                db.SaveChanges();
                return 1;
            }

            var logFirst = log.First();

            if (a == 0) // Add khi kiểm tra đã có cache
            {
                LogTimeCache AddLogTime = new LogTimeCache()
                {
                    CreatedDate = DateTime.Now,
                    PreOder = logFirst.PreOder + 1,
                    CacheType = (int)CacheType.MemoryCache_GetAllDish,
                    Description = "Cache All Dish",
                };
                db.LogTimeCaches.Add(AddLogTime);
                return db.SaveChanges();
            }

            LogTimeCache logTime = new LogTimeCache()
            {
                CreatedDate = DateTime.Now,
                PreOder = logFirst.PreOder + 1,
                CacheType = (int)CacheType.MemoryCache_GetAllDish,
                Description = "Cache All Dish",
            };
            db.LogTimeCaches.Add(logTime);
            db.SaveChanges();

            var logExist = db.LogTimeCaches.Where(x => x.CacheType == (int)CacheType.MemoryCache_GetAllDish).ToList().OrderByDescending(x => x.id);

            if (logExist.Count() > 3)
            {
                var lstLog = logExist.Take(3);
                var log1 = Convert.ToDateTime(lstLog.First().CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss"));
                var log3 = Convert.ToDateTime(lstLog.Last().CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss"));
                TimeSpan timeSpan = log1 - log3;
                var numberSecond = (int)timeSpan.TotalSeconds;
                if (numberSecond < 60)
                {
                    var logDelete = logExist.Skip(1).ToList();
                    db.LogTimeCaches.RemoveRange(logDelete);
                    db.SaveChanges();
                    return 2;
                }
            }
            return 3;
        }
    }
}