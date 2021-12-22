using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.InfoModel
{
    public class RegisterByMenuInfo
    {
        public int? MenuId { get; set; }
        public List<SelectListItem> lstMenu { get; set; }
    }
}