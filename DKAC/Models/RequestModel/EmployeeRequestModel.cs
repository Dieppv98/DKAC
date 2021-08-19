using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.RequestModel
{
    public class EmployeeRequestModel : BaseModel
    {
        [AllowHtml]
        public string Keywords { get; set; }
        public List<Employee> data { get; set; }
    }
}