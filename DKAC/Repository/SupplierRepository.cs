using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using DKAC.Models.Enum;

namespace DKAC.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        DKACDbContext db = new DKACDbContext();
        public int Add(SupplierInfo model)
        {
            try
            {
                Supplier sup = new Supplier();
                sup.SupplierCode = model.SupplierCode;
                sup.SupplierName = model.SupplierName;
                sup.Address = model.Address;
                sup.Tel = model.Tel;
                sup.CreatedDate = DateTime.Now;
                sup.ModifyDate = DateTime.Now;
                sup.CreatedBy = model.CreatedBy;
                sup.IsDeleted = 0;
                db.Suppliers.Add(sup);
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Delete(Supplier sup)
        {
            var info = db.Suppliers.Where(x => x.IsDeleted == 0 && x.id == sup.id).FirstOrDefault();
            if (info != null)
            {
                info.IsDeleted = 1;
                info.ModifyDate = DateTime.Now;
                db.SaveChanges();
                return 1;
            }
            return 0;
        }

        public List<Supplier> GetAll()
        {
            return db.Suppliers.Where(x => x.IsDeleted == 0).ToList();
        }

        public SupplierRequestModel GetAllSupplier(string KeySearch, int page, int pageSize)
        {
            SupplierRequestModel request = new SupplierRequestModel();
            List<Supplier> lst = new List<Supplier>();
            lst = db.Suppliers.Where(x => x.IsDeleted == 0).ToList();
            request.page = page;
            request.pageSize = pageSize;

            int startRow = (page - 1) * pageSize;
            if (!string.IsNullOrEmpty(KeySearch))
            {
                request.totalRecord = lst.Where(x => x.SupplierCode.Contains(KeySearch) || x.SupplierName.Contains(KeySearch)).ToList().Count;
                request.data = lst.Where(x => x.SupplierCode.Contains(KeySearch) || x.SupplierName.Contains(KeySearch)).OrderBy(x => x.ModifyDate).Skip(startRow).Take(pageSize).ToList();
            }
            else
            {
                request.totalRecord = lst.Count;
                request.data = lst.OrderBy(x => x.ModifyDate).Skip(startRow).Take(pageSize).ToList();
            }

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
            request.KeyWord = KeySearch;
            return request;
        }

        public Supplier GetByCodeId(string Code, int id)
        {
            Supplier sup = db.Suppliers.Where(x => x.IsDeleted == 0 && x.SupplierCode == Code && x.id != id).FirstOrDefault();
            return sup;
        }

        public Supplier GetById(int? id, int? currentUser)
        {
            var user = db.Users.Where(x => x.id == currentUser).FirstOrDefault();
            Supplier sup = db.Suppliers.Where(x => x.IsDeleted == 0 && x.id == id).FirstOrDefault();
            if (sup == null) return new Supplier();
            //if (user.UserName != "admin")
            //{
            //    if (sup.CreatedBy != currentUser)
            //    {
            //        return null;
            //    }
            //}
            return sup;
        }

        public int Update(SupplierInfo model)
        {
            try
            {
                Supplier sup = db.Suppliers.Where(x => x.IsDeleted == 0 && x.id == model.id).FirstOrDefault();
                sup.SupplierCode = model.SupplierCode;
                sup.SupplierName = model.SupplierName;
                sup.Address = model.Address;
                sup.Tel = model.Tel;
                sup.ModifyDate = DateTime.Now;
                sup.IsDeleted = 0;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int UpdateCacheGetAllSupplier(int a)
        {
            var logtest = db.LogTimeCaches.Where(x => x.CacheType == (int)CacheType.MemoryCache_GetAllSupplier).ToList();
            var log = db.LogTimeCaches.Where(x => x.CacheType == (int)CacheType.MemoryCache_GetAllSupplier).ToList().OrderByDescending(x => x.id);
            // add bản ghi đầu tiên
            if (logtest == null || logtest.Count() == 0)
            {
                LogTimeCache newLogTime = new LogTimeCache()
                {
                    CreatedDate = DateTime.Now,
                    PreOder = 1,
                    CacheType = (int)CacheType.MemoryCache_GetAllSupplier,
                    Description = "Cache All Supplier",
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
                    CacheType = (int)CacheType.MemoryCache_GetAllSupplier,
                    Description = "Cache All Supplier",
                };
                db.LogTimeCaches.Add(AddLogTime);
                return db.SaveChanges();
            }

            LogTimeCache logTime = new LogTimeCache()
            {
                CreatedDate = DateTime.Now,
                PreOder = logFirst.PreOder + 1,
                CacheType = (int)CacheType.MemoryCache_GetAllSupplier,
                Description = "Cache All Supplier",
            };
            db.LogTimeCaches.Add(logTime);
            db.SaveChanges();

            var logExist = db.LogTimeCaches.Where(x => x.CacheType == (int)CacheType.MemoryCache_GetAllSupplier).ToList().OrderByDescending(x => x.id);

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