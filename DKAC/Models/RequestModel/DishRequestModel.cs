using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.RequestModel
{
    public class DishRequestModel : BaseModel
    {
        public Dish KeySearch { get; set; }
        public int? FromCost { get; set; }
        public int? ToCost { get; set; }
        public List<DishInfo> data { get; set; }
        public int? hasDataPermission { get; set; }
    }
}