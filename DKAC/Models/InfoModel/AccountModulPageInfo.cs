using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class AccountModulPageInfo
    {
        public List<PermissionActionInfo> ListPerAction { get; set; }

        public List<Modul> ListModul { get; set; }
        public List<Page> ListPage { get; set; }

    }
}