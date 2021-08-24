using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class LoadNotifyInfo
    {
        public List<NotifyInfo> lstNotiNew { get; set; }
        public List<NotifyInfo> lstNotiOld { get; set; }
    }
}