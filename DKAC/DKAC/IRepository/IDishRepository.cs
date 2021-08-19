using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.IRepository
{
    public interface IDishRepository
    {
        int Add(DishInfo model);
        int Update(DishInfo model);
        DishRequestModel GetAllDish(Dish KeySearch, int? FromCost, int? ToCost, int page, int pageSize);
        int Delete(Dish dish);
        Dish GetById(int id);
        Dish GetByCodeId(string Code, int id);
    }
}