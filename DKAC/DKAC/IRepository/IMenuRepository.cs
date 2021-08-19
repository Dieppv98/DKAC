using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.IRepository
{
    public interface IMenuRepository
    {
        int Add(MenuInfo model);
        int Update(MenuInfo model);
        int Delete(Menu menu);
        MenuRequestModel GetAllMenu(string KeySearch, int page, int pageSize);
        Menu GetById(int id);
        Menu GetByCodeId(string Code, int id);
    }
}