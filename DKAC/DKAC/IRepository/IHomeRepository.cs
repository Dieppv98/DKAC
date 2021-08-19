using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.IRepository
{
    public interface IHomeRepository
    {
        List<Dish> GetAllDishs(int? typeId);
        List<DishType> GetAllDishTypes();
    }
}