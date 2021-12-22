using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.RequestModel
{
    public class MenuRequestModel : BaseModel
    {
        [AllowHtml]
        public override object RouteValues => new
        {
            this.KeyWord
        };
        public string KeyWord { get; set; }
        public List<Menu> data { get; set; }
    }
}