using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DKAC.IRepository
{
    public interface IRegisterRepository
    {
        int RegisterByPersonal(Register model);
        int RegisterByGroup(List<Register> model);
        List<DishInfo> GetDishs();
        Room GetRoomById(int? id);
        Dish GetDishById(int? id);
        List<RegisterByPersonalInfo> GetAllRegister(int emId, DateTime regDate);
        int DeleteReg(int? id);
        List<Menu> GetMenuByUserId(int id);
    }
}