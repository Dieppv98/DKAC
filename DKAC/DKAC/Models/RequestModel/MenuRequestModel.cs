using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DKAC.Models.RequestModel
{
    public class MenuRequestModel : BaseModel
    {
        public string KeyWord { get; set; }
        public List<Menu> data { get; set; }
    }
}