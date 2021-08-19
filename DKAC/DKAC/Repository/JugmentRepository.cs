using DKAC.Common;
using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Repository
{
    public class JugmentRepository : IJugmentRepository
    {
        DKACDbContext db = new DKACDbContext();

        public int Jugment(Jugment model)
        {
            try
            {
                model.IsDeleted = 0;
                db.Jugments.Add(model);
                var dish = db.Dishes.FirstOrDefault(x => x.IsDeleted == 0 && x.id == model.DishId);
                double totalPoint = (double)(dish.JugmentPoint + model.Point);
                dish.JugmentPoint = (float)(totalPoint / 2);
                dish.JugmentQty++;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public DishInfo GetDish(int? dishId)
        {
            var query = from d in db.Dishes
                        where d.IsDeleted == 0 && d.id == dishId
                        join dt in db.DishTypes on d.DishTypeId equals dt.id into ldt
                        from ld in ldt.DefaultIfEmpty()
                        select new DishInfo()
                        {
                            id = d.id,
                            Cost = d.Cost,
                            Description = d.Description,
                            DishCode = d.DishCode,
                            DishName = d.DishName,
                            DishTypeId = d.DishTypeId,
                            DishTypeName = ld.DishTypeName,
                            Image = d.Image,
                            JugmentPoint = d.JugmentPoint,
                            JugmentQty = d.JugmentQty,
                            RegisterQty = d.RegisterQty,
                            SupplierId = d.SupplierId,
                        };
            return query.FirstOrDefault();
        }
    }
}