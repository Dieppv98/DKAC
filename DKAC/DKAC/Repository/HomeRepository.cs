using DKAC.IRepository;
using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public class HomeRepository : IHomeRepository
    {
        DKACDbContext db = new DKACDbContext();

        public List<Dish> GetAllDishs(int? typeId)
        {
            var query = from d in db.Dishes
                        where d.IsDeleted == 0
                        select (d);
            if (typeId != null)
            {
                query = query.Where(x => x.DishTypeId == typeId);
            }
            return query.ToList();
        }

        public List<DishType> GetAllDishTypes()
        {
            return db.DishTypes.Where(x => x.IsDeleted == 0).ToList();
        }

        public Jugment GetJug(int? dishId, int? emId)
        {
            return db.Jugments.FirstOrDefault(x => x.IsDeleted == 0 && x.DishId == dishId && x.EmployeeId == emId);
        }
    }
}