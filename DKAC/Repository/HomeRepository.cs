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
            DateTime now = DateTime.Now.Date;

            var thisWeekStart = now.AddDays(-(int)now.DayOfWeek);//ngày bắt đầu tuần(chủ nhật) theo now
            var thisWeekEnd = thisWeekStart.AddDays(7);//ngày kết thúc tuần(thứ 7) theo now

            return db.Jugments.AsNoTracking().FirstOrDefault(x => x.IsDeleted == 0 && x.DishId == dishId && x.EmployeeId == emId && x.JugmentDate >= thisWeekStart && x.JugmentDate < thisWeekEnd);
        }

        public List<Dish> GetListAllDish()
        {
            var query = from d in db.Dishes
                        where d.IsDeleted == 0
                        select (d);
            return query.ToList();
        }
    }
}