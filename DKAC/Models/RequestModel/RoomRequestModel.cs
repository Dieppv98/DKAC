using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DKAC.Models.RequestModel
{
    public class RoomRequestModel : BaseModel
    {
        [AllowHtml]
        public override object RouteValues => new
        {
            this.Keywords
        };
        public string Keywords { get; set; }
        public List<RoomInfo> data { get; set; }
    }
}