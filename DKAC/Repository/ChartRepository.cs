using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public class ChartRepository : IChartRepository
    {
        DKACDbContext db = new DKACDbContext();

        public ChartInfo LoadChart()
        {
            int numberSup = 0;
            int numberDish = 0;
            var numberAllSup = db.LogTimeCaches.Where(x => x.CacheType == (int)CacheType.MemoryCache_GetAllSupplier).OrderByDescending(x => x.PreOder).ToList();
            var numberAllDish = db.LogTimeCaches.Where(x => x.CacheType == (int)CacheType.MemoryCache_GetAllDish).OrderByDescending(x => x.PreOder).ToList();

            if (numberAllSup.Count > 0) numberSup = numberAllSup.FirstOrDefault().PreOder ?? 0;
            if (numberAllDish.Count > 0) numberDish = numberAllDish.FirstOrDefault().PreOder ?? 0;

            int numSum = numberSup + numberDish;
            ChartInfo chartInfo = new ChartInfo()
            {
                NumberGetAllDish = numberDish,
                NumberGetAllSupplier = numberSup,
                NumberSum = numSum,
                PercentGetAllDish = ((double)numberDish / (double)numSum * 100),
                PercentGetAllSupplier = ((double)numberSup / (double)numSum * 100),
            };
            return chartInfo;
        }
    }
}