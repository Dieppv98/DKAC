using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.RequestModel
{
    public class HomeRequestModel : BaseModel
    {
        public List<DishInfo> lstDish { get; set; }
        public List<Dish> lstAllDish { get; set; }

        public List<DishType> lstDishType { get; set; }
    }
}