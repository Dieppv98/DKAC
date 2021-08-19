using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.IRepository
{
    public interface IChartRepository
    {
        ChartInfo LoadChart();
    }
}