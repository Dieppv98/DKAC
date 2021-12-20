using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public interface IEmployeeRepository
    {
        int Add(User employee);
        int Delete(int id);
        UserInfo GetById(int? id);
        UserRequestModel GetListAllEmployee(User user, string KeySearch, int page, int pageSize);
        int Update(UserInfo employee);
        User GetByUserName(string UserName, int? id);
        List<Room> GetAllRoom();

    }
}