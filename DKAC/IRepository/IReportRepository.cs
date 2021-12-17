using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.IRepository
{
    public interface IReportRepository
    {
        List<RegisterByPersonalInfo> GetListRegisterReport(DateTime? fDate, DateTime? tDate, int? emId, string dish);
    }
}